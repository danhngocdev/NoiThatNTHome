using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVG.WIS.Business.Activities;
using DVG.WIS.Business.Authenticator;
using DVG.WIS.Core.Constants;
using DVG.WIS.Core.Enums;
using DVG.WIS.DAL.Users;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;

namespace DVG.WIS.Business.Users
{
    public class UserBo: IUserBo
    {
        private IUserDAL _userDal;
        private IActivityBo _activityBo;

        public UserBo(IUserDAL userDal, IActivityBo activityBo)
        {
            this._userDal = userDal;
            this._activityBo = activityBo;
        }

        public UserOnList GetById(int userId)
        {
            try
            {
                return _userDal.GetById(userId);
            }
            catch (Exception ex)
            {
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, userId);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return new UserOnList();
        }

        public Entities.User GetByUserName(string userName)
        {
            try
            {
                return _userDal.GetByUserName(userName);
            }
            catch (Exception ex)
            {
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, userName);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return new UserOnList();
        }

        public Entities.User ValidateLogin(string userName, string password)
        {
            var user = GetUserInfoByAccountName(userName);
            if (user != null)
            {
                if (password == user.Password)
                    return user;
            }
            user = GetUserInfoByEmail(userName);
            if (user != null)
            {
                if (password == user.Password)
                    return user;
            }
            return null;
        }

        public Entities.User GetUserInfoByEmail(string email)
        {
            return _userDal.GetUserInfoByEmail(email);
        }

        public WIS.Entities.User GetUserInfoByAccountName(string accountName)
        {
            return _userDal.GetByUserName(accountName);
        }

        public ErrorCodes Update(WIS.Entities.User user, ref int userId)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                if (!AuthenService.IsLogin())
                {
                    return ErrorCodes.NotLogin;
                }

                if (user == null || string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
                {
                    return ErrorCodes.BusinessError;
                }

                userId = _userDal.Update(user);
            }
            catch (Exception ex)
            {
                errorCodes = ErrorCodes.BusinessError;
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, user);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return errorCodes;
        }

        public ErrorCodes Update(WIS.Entities.User user)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                if (!AuthenService.IsLogin())
                {
                    return ErrorCodes.NotLogin;
                }

                if (user == null || string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
                {
                    return ErrorCodes.BusinessError;
                }

                _userDal.Update(user);
            }
            catch (Exception ex)
            {
                errorCodes = ErrorCodes.BusinessError;
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, user);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return errorCodes;
        }

        public ErrorCodes UpdateLastLogin(int userId)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                var lastLogin = Utils.DateTimeToUnixTime(DateTime.Now);

                _userDal.UpdateLastLogin(userId, lastLogin);
            }
            catch (Exception ex)
            {
                errorCodes = ErrorCodes.BusinessError;
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, userId);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return errorCodes;
        }

        public ErrorCodes ChangePassword(string username, string currentPassword, string passsword, string confirmPassword)
        {
            try
            {
                currentPassword = Crypton.Encrypt(currentPassword);
                passsword = Crypton.Encrypt(passsword);
                confirmPassword = Crypton.Encrypt(confirmPassword);

                var userInfo = _userDal.GetByUserName(username);

                if (userInfo == null || userInfo.UserId <= 0)
                {
                    return ErrorCodes.AccountNotExists;
                }
                if (!passsword.Equals(confirmPassword))
                {
                    return ErrorCodes.AccountPasswordNotMatch;
                }
                if (!userInfo.Password.Equals(currentPassword))
                {
                    return ErrorCodes.AccountLoginInvalidPassword;
                }

                userInfo.Password = passsword;
                userInfo.CreatedDate = DateTime.Now;
                userInfo.CreatedDateSpan = Utils.DateTimeToUnixTime(DateTime.Now);
                userInfo.Password = passsword;

                _userDal.Update(userInfo);

                // Create activity foor change pass
                var userInfoLogin = AuthenService.GetUserLogin();
                var modelActivityCom = new ActivityModel()
                {
                    ActionTimeStamp = Utils.DateTimeToUnixTime(DateTime.Now),
                    UserId = userInfoLogin.UserId,
                    ActionDate = DateTime.Now,
                    ActionText = StringUtils.GetEnumDescription(ActivityTypeEnum.ChangePassword),
                    ActionType = (int)ActivityTypeEnum.ChangePassword,
                    ObjectId = 0
                };
                _activityBo.Insert(modelActivityCom);

                return ErrorCodes.Success;
            }
            catch (Exception ex)
            {
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, username);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return ErrorCodes.UnknowError;
        }


        public IEnumerable<UserOnList> GetList(string keyword, int? authGroupId = 0, int? pageIndex = 1, int? pageSize = 15)
        {
            try
            {
                return _userDal.GetList(keyword, authGroupId, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, keyword, authGroupId, pageIndex, pageSize);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return null;
            }
        }


        public IEnumerable<WIS.Entities.User> GetListInBank(int bankId, int status, int userType)
        {
            try
            {
                return _userDal.GetListInBank(bankId, status, userType);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return null;
            }
        }

        public string GenerateEmailCreateAcount(string loginLink, string fullName, string userName, string pass, string accountType)
        {
            StringBuilder contentBuilder = new StringBuilder();
            contentBuilder.Append("<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"600\" style=\"margin: 0 auto;\">");
            contentBuilder.Append("<tbody>");
            contentBuilder.Append("<tr><td>");
            contentBuilder.Append("<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\">");
            contentBuilder.Append("<tbody>");
            contentBuilder.Append("<tr><td width=\"60\"></td>");
            contentBuilder.Append("<td>");
            contentBuilder.Append("<h4 style=\"font-family: arial; font-size: 26px; font-weight: bold; color: #243b81; text-transform: uppercase; margin: 0 0 20px 0; padding: 0;\">THÔNG TIN TÀI KHOẢN</h4>");
            contentBuilder.AppendFormat("<font style=\"font-family: arial; font-size: 16px; color: #;323c3f\">Xin chào {0},</font>", fullName);
            contentBuilder.AppendFormat("<br><font style=\"font-family: arial; font-size: 16px; color: #;323c3f\">Quản trị viên vừa tạo cho bạn tài khoản trên hệ thống {0}. Bạn hãy truy cập vào link sau để kiểm tra tài khoản và thay đổi mật khẩu:</font>", StaticVariable.Domain);
            contentBuilder.Append("<br><br>");
            contentBuilder.Append("<div style=\"text-align: center;\">");
            contentBuilder.AppendFormat("<a style=\"display: inline-block; font-family: arial; color: #fff; font-size: 20px; font-weight: bold; text-transform: uppercase; text-decoration: none !important; padding: 10px 50px; background-color: #00a9f4; border-radius: 6px; -moz-border-radius: 6px; -webkit-border-radius: 6px;\" href=\"{0}\">Link đăng nhập hệ thống</a>", loginLink);
            contentBuilder.Append("</div><br>");
            contentBuilder.AppendFormat("<font style=\"font-family: arial; font-size: 16px; color: #;323c3f\"> Tên tài khoản : {0}</font>", userName);
            contentBuilder.AppendFormat("<br><font style=\"font-family: arial; font-size: 16px; color: #;323c3f\"> Mật khẩu : {0}</font>", pass);
            contentBuilder.AppendFormat("<br><font style=\"font-family: arial; font-size: 16px; color: #;323c3f\"> Loại tài khoản : {0}</font>", accountType);
            contentBuilder.AppendFormat("<br><br><font style=\"font-family: arial; font-size: 16px; color: #;323c3f\">Chúc bạn có những trải nhiệm thú vị với {0}.</font>", StaticVariable.Domain);
            contentBuilder.Append("<br><font style=\"font-family: arial; font-size: 16px; color: #;323c3f\">Trân trọng,</font>");
            contentBuilder.Append("<br><br><font style=\"font-family: arial; font-size: 16px; font-weight: bold; color: #;323c3f\">Ban biên tập trường THCS Nguyễn Du</font>");
            contentBuilder.Append("</td>");
            contentBuilder.Append("<td width=\"60\"></td>");
            contentBuilder.Append("</tr>");
            contentBuilder.Append("</tbody></table>");
            contentBuilder.Append("</td></tr>");
            contentBuilder.Append("</tbody></table>");

            return contentBuilder.ToString();
        }

        public List<Entities.User> GetAll()
        {
            try
            {
                return _userDal.GetAll();
            }
            catch (Exception ex)
            {
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return null;
            }
        }
    }
}
