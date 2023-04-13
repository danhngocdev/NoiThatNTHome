using DVG.WIS.Business.Activities;
using DVG.WIS.Business.Users;
using DVG.WIS.Business.AuthAction;
using DVG.WIS.Business.AuthGroupActionMapping;
using DVG.WIS.Business.AuthGroupNewsStatusMapping;
using DVG.WIS.Business.AuthGroupUserMapping;
using DVG.WIS.Caching;
using DVG.WIS.Caching.Cached;
using DVG.WIS.Caching.Cached.Implements;
using DVG.WIS.Caching.DTO.Entities;
using DVG.WIS.Core;
using DVG.WIS.Core.Constants;
using DVG.WIS.Core.Enums;
using DVG.WIS.DAL.Activities;
using DVG.WIS.DAL.AuthAction;
using DVG.WIS.DAL.AuthGroupActionMapping;
using DVG.WIS.DAL.AuthGroupNewsStatusMapping;
using DVG.WIS.DAL.AuthGroupUserMapping;
using DVG.WIS.DAL.Users;
using DVG.WIS.Entities;
using DVG.WIS.PublicModel.CMS;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace DVG.WIS.Business.Authenticator
{
    public class AuthenService : IRequiresSessionState
    {
        private static IUserBo _userBo;
        private static IActivityBo _activityBo;
        private static IAuthGroupUserMappingBo _authGroupUserMappingBo;
        private static IAuthGroupActionMappingBo _authGroupActionMappingBo;
        private static IAuthGroupNewsStatusMappingBo _authGroupNewsStatusMappingBo;
        private static IAuthActionBo _authActionBo;
        private static ICached _cacheClient;
        private static int _weekExpiredInMinute = StaticVariable.WeekCacheTime;
        static AuthenService()
        {
            _userBo = new UserBo(new UserDAL(), new ActivityBo(new ActivityDal()));
            _activityBo = new ActivityBo(new ActivityDal());
            _authGroupUserMappingBo = new AuthGroupUserMappingBo(new AuthGroupUserMappingDal());
            _authGroupActionMappingBo = new AuthGroupActionMappingBo(new AuthGroupActionMappingDal());
            _authGroupNewsStatusMappingBo = new AuthGroupNewsStatusMappingBo(new AuthGroupNewsStatusMappingDal());
            _authActionBo = new AuthActionBo(new AuthActionDal());

            CachingConfigModel config = new CachingConfigModel()
            {
                IpServer = AppSettings.Instance.GetString("RedisIP"),
                Port = AppSettings.Instance.GetInt32("RedisPort"),
                DB = AppSettings.Instance.GetInt32("RedisDB"),
                ConnectTimeout = AppSettings.Instance.GetInt32("RedisTimeout", 600),
                RedisSlotNameInMemory = AppSettings.Instance.GetString("RedisSlotName", "RedisSlotNameForCMS")
            };

            _cacheClient = new RedisCached(config);
        }

        public static bool IsLogin()
        {
            try
            {
                var isLogin = HttpContext.Current.Request.IsAuthenticated;
                return isLogin;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return false;
            }
        }

        public static UserLogin GetUserLogin()
        {
            UserLogin userInfo = new UserLogin();
            try
            {
                if (IsLogin())
                {
                    FormsIdentity identity = HttpContext.Current.User.Identity as FormsIdentity;
                    if (identity != null)
                    {
                        string userData = identity.Ticket.UserData;
                        if (!string.IsNullOrEmpty(userData))
                        {
                            try
                            {
                                userInfo = NewtonJson.Deserialize<UserLogin>(userData);
                            }
                            catch (Exception ex)
                            {
                                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                            }
                        }

                        if (userInfo == null || userInfo.UserId <= 0)
                        {
                            Entities.User existsUsers = _userBo.GetUserInfoByAccountName(identity.Name);
                            if (existsUsers != null && existsUsers.UserId > 0)
                            {
                                var userActived = existsUsers.Status != (int)UserStatusAdmin.Actived;

                                if (!userActived) return userInfo = new UserLogin();

                                userInfo = new UserLogin(existsUsers);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return userInfo;
        }

        public static ResponseData Login(string userName, string password, bool isSavedPassword = false,
            string secureCode = "")
        {
            var responseData = new ResponseData();

            int num = 0;
            HttpContext current = HttpContext.Current;
            if (current != null)
            {
                if ((current.Session[Const.SessionLoginFailName] != null) &&
                    !string.IsNullOrEmpty(current.Session[Const.SessionLoginFailName].ToString()))
                {
                    num = int.Parse(current.Session[Const.SessionLoginFailName].ToString());
                }

                if ((num >= 5) &&
                    (((string.IsNullOrEmpty(secureCode)) || (current.Session[Const.SessionCaptcharName] == null)) ||
                     (current.Session[Const.SessionCaptcharName].ToString().ToLower() != secureCode.ToLower())))
                {
                    responseData.TotalRow = num;
                    responseData.ErrorCode = 4;
                    responseData.Message = "Tài khoản tạm thời bị khóa do đặng nhập sai nhiều lần";
                    responseData.Success = false;
                    return responseData;
                }
            }

            if (string.IsNullOrEmpty(userName))
            {
                responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.AccountLoginInvalidUserName);
            }
            if (string.IsNullOrEmpty(password))
            {
                responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.AccountLoginInvalidPassword);
            }

            password = Crypton.Encrypt(password);
            //Fix bug cho Admin
            //password = "yTvYCERxr4KMSEpBIgALRw==";
            Entities.User userEntity = _userBo.ValidateLogin(userName, password);
            if (userEntity != null && userEntity.UserId > 0)
            {
                if (userEntity.Status == (int)UserStatusAdmin.Deactived)
                {
                    responseData.Success = false;
                    responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.AccountLoginUserBanned);
                    responseData.ErrorCode = (int)ErrorCodes.AccountLoginUserBanned;
                    return responseData;
                }

                if (userEntity.Status == (int)UserStatusAdmin.Deleted)
                {
                    responseData.Success = false;
                    responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.AccountLoginUserRemoved);
                    responseData.ErrorCode = (int)ErrorCodes.AccountLoginUserRemoved;
                    return responseData;
                }

                responseData.Success = true;
                responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.Success);
                responseData.ErrorCode = (int)ErrorCodes.Success;
                responseData.Data = userEntity;

                DoLogin(userName, true);

                return responseData;
            }

            responseData.Success = false;
            responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.AccountLoginWrongUserNameOrPassword);
            responseData.ErrorCode = (int)ErrorCodes.AccountLoginWrongUserNameOrPassword;
            responseData.TotalRow = num;
            num++;
            if (current != null)
            {
                current.Session[Const.SessionLoginFailName] = num.ToString();
                current.Session.Timeout = 3;
            }

            return responseData;
        }

        public static ResponseData LoginForCms(string userName, string password, bool isSavedPassword = false,
            string secureCode = "")
        {
            var responseData = Login(userName, password, isSavedPassword, secureCode);

            if (responseData.Success)
            {
                if (!responseData.Success)
                {
                    responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.AccessDenined);
                    FormsAuthentication.SignOut();
                }
                else
                {
                    Entities.User userInfo = _userBo.GetUserInfoByAccountName(userName);
                    var modelActivityCom = new ActivityModel()
                    {
                        ActionTimeStamp = Utilities.Utils.DateTimeToUnixTime(DateTime.Now),
                        UserId = userInfo.UserId,
                        ActionDate = DateTime.Now,
                        ActionText = StringUtils.GetEnumDescription(ActivityTypeEnum.Login),
                        ActionType = (int)ActivityTypeEnum.Login,
                        ObjectId = 0
                    };

                    // lấy user permission
                    if (userInfo.UserId > 0)
                    {
                        SaveAuthGroupActionMappingOnCache(userInfo.UserId);
                    }

                    _activityBo.Insert(modelActivityCom);
                }
            }

            return responseData;
        }

        public static ResponseData Logout()
        {
            FormsAuthentication.SignOut();

            var userInfo = AuthenService.GetUserLogin();
            var modelActivityCom = new ActivityModel()
            {
                ActionTimeStamp = Utilities.Utils.DateTimeToUnixTime(DateTime.Now),
                UserId = userInfo.UserId,
                ActionDate = DateTime.Now,
                ActionText = StringUtils.GetEnumDescription(ActivityTypeEnum.Logout),
                ActionType = (int)ActivityTypeEnum.Logout,
                ObjectId = 0
            };
            _activityBo.Insert(modelActivityCom);

            return new ResponseData
            {
                Success = true,
                Message = StringUtils.GetEnumDescription(ErrorCodes.Success)
            };
        }

        public static void DoLogin(string accountName, bool saveCookie)
        {
            if (!IsLogin())
            {
                try
                {
                    bool @bool = AppSettings.Instance.GetBool(Const.DebugMode);
                    Entities.User userInfo = _userBo.GetUserInfoByAccountName(accountName);
                    HttpContext current = HttpContext.Current;
                    if ((userInfo != null) && (userInfo.UserId > 0))
                    {
                        UserLogin login = new UserLogin(userInfo);

                        string userData = NewtonJson.Serialize(login);
                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, login.Email, DateTime.Now, DateTime.Now.AddHours(4.0), false, userData, FormsAuthentication.FormsCookiePath);
                        string str2 = FormsAuthentication.Encrypt(ticket);
                        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, str2);
                        //if (!@bool)
                        //{
                        //    cookie.Domain = FormsAuthentication.CookieDomain;
                        //}
                        current.Response.Cookies.Add(cookie);
                        if (saveCookie)
                        {
                            if ((current.Request.Cookies[Const.KeyAccountId] != null) && !string.IsNullOrEmpty(current.Request.Cookies[Const.KeyAccountId].Value))
                            {
                                current.Response.Cookies.Remove(Const.KeyAccountId);
                            }
                            else
                            {
                                HttpCookie cookie2 = new HttpCookie(Const.KeyAccountId)
                                {
                                    Value = Crypton.EncryptByKey(userData, Const.KeyAccountId),
                                    Expires = DateTime.Now.AddDays(5.0)
                                };
                                current.Response.Cookies.Add(cookie2);
                            }
                        }



                        if (userInfo.UserId > 0)
                        {
                            // lưu last login
                            _userBo.UpdateLastLogin(userInfo.UserId);
                            // lấy user permission
                            SaveAuthGroupActionMappingOnCache(userInfo.UserId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                }
            }
        }

        public static AuthUserPermission SaveAuthGroupActionMappingOnCache(int userId)
        {
            var objPermission = new AuthUserPermission();
            var lstAuthGroupUserMapping = _authGroupUserMappingBo.GetByUserId(userId);
            if (lstAuthGroupUserMapping != null && lstAuthGroupUserMapping.Any())
            {
                var authGroupId = lstAuthGroupUserMapping.FirstOrDefault().AuthGroupId;
                if (authGroupId > 0)
                {
                    // action
                    var lstAction = _authActionBo.GetAll();
                    var lstAuthGroupActionMapping = _authGroupActionMappingBo.GetByGrouId(authGroupId);
                    if (lstAuthGroupActionMapping != null && lstAuthGroupActionMapping.Any())
                    {
                        foreach (var actionMapping in lstAuthGroupActionMapping)
                        {
                            var action = lstAction.Where(x => x.Id == actionMapping.AthActionId).FirstOrDefault();
                            if (action != null) objPermission.LstPermissionAction.Add(action);
                        }
                    }

                    // category

                    // news status
                    var lstAuthGroupNewsStatusMapping = _authGroupNewsStatusMappingBo.GetByGrouId(authGroupId);
                    if (lstAuthGroupNewsStatusMapping != null && lstAuthGroupNewsStatusMapping.Any())
                    {
                        foreach (var newsStatusMapping in lstAuthGroupNewsStatusMapping)
                        {
                            objPermission.LstPermissionNewsStatus.Add(newsStatusMapping.Status);
                        }
                    }
                }
            }

            if (objPermission != null)
            {
                string keyCached = KeyCacheHelper.GenCacheKey(ConstKeyCached.AuthUserPermission, userId);
                _cacheClient.Add(keyCached, objPermission, _weekExpiredInMinute);
            }
            return objPermission;
        }

        public static AuthUserPermission GetAuthGroupActionMappingOnCache(int userId)
        {
            string keyCached = KeyCacheHelper.GenCacheKey(ConstKeyCached.AuthUserPermission, userId);

            var obj = _cacheClient.Get<AuthUserPermission>(keyCached);

            // bật cache lên thì k cần đoạn này
            if (obj == null)
            {
                obj = AuthenService.SaveAuthGroupActionMappingOnCache(userId);
            }

            return obj;
        }

        public static bool DeleteAuthGroupActionMappingOnCache(int userId)
        {
            string keyCached = KeyCacheHelper.GenCacheKey(ConstKeyCached.AuthUserPermission, userId);

            return _cacheClient.Remove(keyCached);
        }

        public static ResponseData CheckPermissionNewsStatus(int currentStatus, bool isAjax = true)
        {
            var responseData = new ResponseData();
            var isValid = false;
            var user = AuthenService.GetUserLogin();
            string userName = user.UserName;
            var isAdminAccount = userName.Equals(AppSettings.Instance.GetString(Const.GodAdminAccount));
            if (isAdminAccount) isValid = true;

            if (!isValid)
            {
                var userPermission = AuthenService.GetAuthGroupActionMappingOnCache(user.UserId);

                // bật cache lên thì k cần đoạn này
                if (userPermission == null)
                {
                    userPermission = AuthenService.SaveAuthGroupActionMappingOnCache(user.UserId);
                }

                if (userPermission != null && userPermission.LstPermissionNewsStatus != null)
                {
                    isValid = userPermission.LstPermissionNewsStatus.Any(x => x == currentStatus);
                };
            }

            if (isValid)
            {
                responseData.Success = true;
            }
            else
            {
                if (isAjax)
                {
                    responseData.Success = false;
                    responseData.Message = "Bạn không có quyền thực hiện chức năng.";
                }
                else
                {
                    HttpContext.Current.Response.Redirect("/permission-denied");
                }
            }
            return responseData;
        }

        public static StatusOfNewsPermission ProcessPermissionStatusOfNews(int userId)
        {
            var result = new StatusOfNewsPermission();
            var lstPermission = GetAuthGroupActionMappingOnCache(userId);
            if (lstPermission != null && lstPermission.LstPermissionNewsStatus != null)
            {
                foreach (var status in lstPermission.LstPermissionNewsStatus)
                {
                    switch (status)
                    {
                        case (int)NewsStatusEnum.PendingApproved:
                            { result.IsPendingApprove = true; continue; };
                        case (int)NewsStatusEnum.Deleted:
                            { result.IsDelete = true; continue; };
                        case (int)NewsStatusEnum.Published:
                            { result.IsPublish = true; continue; };
                        case (int)NewsStatusEnum.UnPublished:
                            { result.IsUnPublish = true; continue; };
                    }
                }
            }
            return result;
        }


        public static bool IsAccAdmin()
        {
            if (IsLogin())
            {
                try
                {
                    UserLogin user = GetUserLogin();
                    if (null != user)
                    {
                        return user.RoleId == (int)RoleEnum.Administrator;
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                }
            }
            return false;
        }

        public static bool IsSupperAdmin()
        {
            if (IsLogin())
            {
                try
                {
                    UserLogin user = GetUserLogin();
                    if (user == null || user.UserId <= 0) return false;

                    string listSupperAdmin = AppSettings.Instance.GetString("SupperAdmin");
                    if (string.IsNullOrEmpty(listSupperAdmin)) listSupperAdmin = "admin";
                    if (listSupperAdmin.Contains(string.Format(",{0},", user.UserName)))
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                }
            }
            return false;
        }

        public static bool IsEditor(int userId)
        {
            if (IsLogin())
            {
                try
                {
                    UserLogin user = GetUserLogin();
                    if (null != user)
                    {
                        return user.RoleId == (int)RoleEnum.Editor;
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                }
            }
            return false;
        }

        public static bool IsReporter(int userId)
        {
            if (IsLogin())
            {
                try
                {
                    UserLogin user = GetUserLogin();
                    if (null != user)
                    {
                        return user.RoleId == (int)RoleEnum.Reporter;
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                }
            }
            return false;
        }

        public static bool IsClericalSecretary(int userId)
        {
            if (IsLogin())
            {
                try
                {
                    UserLogin user = GetUserLogin();
                    if (null != user)
                    {
                        return user.RoleId == (int)RoleEnum.ClericalSecretary;
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                }
            }
            return false;
        }

        public static bool IsOfficeSecretary(int userId)
        {
            if (IsLogin())
            {
                try
                {
                    UserLogin user = GetUserLogin();
                    if (null != user)
                    {
                        return user.RoleId == (int)RoleEnum.OfficeSecretary;
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                }
            }
            return false;
        }

    }
}
