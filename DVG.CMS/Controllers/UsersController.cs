using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using DVG.WIS.Core.Enums;
using DVG.WIS.Core;
using DVG.WIS.Business.AuthGroup;
using DVG.WIS.Business.AuthGroupUserMapping;
using DVG.WIS.PublicModel.CMS;
using DVG.WIS.Core.Constants;
using DVG.WIS.Business.Users;
using DVG.WIS.Business.Authenticator;

namespace DVG.CMS.Controllers
{
    public class UsersController : Controller
    {
        private IUserBo _userBo;
        private IAuthGroupBo _authGroupBo;
        private IAuthGroupUserMappingBo _authGroupUserMappingBo;

        public UsersController(IUserBo userBo, IAuthGroupBo authGroupBo, IAuthGroupUserMappingBo authGroupUserMappingBo)
        {
            this._userBo = userBo;
            _authGroupBo = authGroupBo;
            _authGroupUserMappingBo = authGroupUserMappingBo;
        }

        [IsValidUrlRequest(KeyName = "UsersController.Index", Description = "Tài khoản - Danh sách")]
        public ActionResult Index()
        {
            return View();
        }

        [IsValidUrlRequest(KeyName = "UsersController.Search", Description = "Tài khoản - Danh sách")]
        public ActionResult Search(UsersSearchModel searchModel)
        {
            ResponseData responseData = new ResponseData();
            var listUsers = _userBo.GetList(searchModel.Keyword, searchModel.UserType, searchModel.PageIndex, searchModel.PageSize);
            if (listUsers != null)
            {
                List<UsersModel> models = listUsers.Select(item => new UsersModel(item)).ToList();
                responseData.Data = models;
                responseData.Success = true;
                responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.Success);
            }
            return Json(responseData);
        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "UsersController.GetUserById", Description = "Tài khoản - Cập nhật")]
        public ActionResult GetUserById(int userId)
        {
            ResponseData responseData = new ResponseData();
            UserOnList users = _userBo.GetById(userId);
            if (users != null)
            {

                UsersModel modelCms = new UsersModel(users);

                // lấy AuthGroup
                var lstAuthGroup = _authGroupUserMappingBo.GetByUserId(userId);
                if (lstAuthGroup != null && lstAuthGroup.Any())
                    modelCms.AuthGroupId = lstAuthGroup.FirstOrDefault().AuthGroupId;

                modelCms.Password = Crypton.Decrypt(modelCms.Password);
                responseData.Data = modelCms;
                responseData.Success = true;
                responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.Success);
            }
            else
            {
                responseData.Success = true;
            }

            return Json(responseData);
        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "UsersController.Update", Description = "Tài khoản - Cập nhật")]
        public ActionResult Update(UsersModel users)
        {
            ResponseData responseData = new ResponseData();

            if (!DVG.WIS.Utilities.Utils.DelayBeforeContinue("SaveUserProfile", 5))
            {

                UserOnList usersModel = _userBo.GetById(users.UserId);
                var userId = 0;
                if (usersModel != null && usersModel.UserId > 0)
                {
                    bool isSendMail = !usersModel.Email.Equals(users.Email);
                    usersModel.FullName = users.FullName;
                    usersModel.Email = users.Email;
                    usersModel.Birthday = users.Birthday;
                    usersModel.Avatar = users.Avatar;
                    usersModel.Address = users.Address;
                    usersModel.Gender = users.Gender;
                    usersModel.UserType = users.UserType;
                    usersModel.Mobile = users.Mobile;

                    ErrorCodes errorCode = _userBo.Update(usersModel, ref userId);

                    // insert AuthGroupUserMapping
                    if (users.AuthGroupId != null && users.AuthGroupId.Value > 0)
                    {
                        UpdateAuthGroupUserMapping(userId, users.AuthGroupId.Value);
                    }

                    responseData.Success = errorCode == ErrorCodes.Success;
                    responseData.Message = StringUtils.GetEnumDescription(errorCode);
                    return Json(responseData);
                }
                else
                {
                    //User usersByName = _userBo.GetByUserName(users.UserName);
                    //if (usersModel == null)
                    //{
                    string strPass = "123456"; //; Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
                    UserOnList usersModelTemp = new UserOnList();
                    usersModelTemp.UserName = users.UserName;
                    usersModelTemp.Password = Crypton.Encrypt(strPass);
                    usersModelTemp.FullName = users.FullName;
                    usersModelTemp.Email = users.Email;
                    usersModelTemp.Birthday = users.Birthday;
                    usersModelTemp.Avatar = users.Avatar;
                    usersModelTemp.Address = users.Address;
                    usersModelTemp.PasswordQuestion = string.Empty;
                    usersModelTemp.PasswordAnswer = string.Empty;
                    usersModelTemp.CreatedDate = DateTime.Now;
                    usersModelTemp.CreatedDateSpan = DVG.WIS.Utilities.Utils.DateTimeToUnixTime(DateTime.Now);
                    usersModelTemp.Status = (int)UserStatusAdmin.Actived;
                    usersModelTemp.UserType = users.UserType;
                    usersModelTemp.Gender = users.Gender;
                    usersModelTemp.Mobile = users.Mobile;

                    ErrorCodes errorCodes = _userBo.Update(usersModelTemp, ref userId);

                    if (errorCodes == ErrorCodes.Success)
                    {
                        // gửi mail cho người dùng được tạo mới
                        //string loginLink = string.Format("{0}/dang-nhap?returnUrl={1}/doi-mat-khau",
                        //    StaticVariable.BaseUrl,
                        //    StaticVariable.BaseUrl);
                        //string accountType = StringUtils.GetEnumDescription((UserTypeEnum)users.UserType.ToLong());
                        //string subject = "[" + StaticVariable.Domain +
                        //                 "] THÔNG TIN TÀI KHOẢN";
                        //string body = _userBo.GenerateEmailCreateAcount(loginLink, users.FullName,
                        //    users.UserName, strPass, accountType);
                        //string result = GoogleMail.SendMail(users.Email, StaticVariable.EmailMaster, subject, body, StaticVariable.EmailNoReply,
                        //    StaticVariable.PassEmailNoReply);
                        //if (string.IsNullOrEmpty(result))
                        //{
                        //    errorCodes = ErrorCodes.SendMailError;
                        //}
                    }

                    // insert AuthGroupUserMapping
                    if (users.AuthGroupId != null && users.AuthGroupId.Value > 0)
                    {
                        UpdateAuthGroupUserMapping(userId, users.AuthGroupId.Value);
                    }

                    responseData.Success = errorCodes == ErrorCodes.Success;
                    responseData.Message = StringUtils.GetEnumDescription(errorCodes);
                    return Json(responseData);
                }
            }

            responseData.Success = false;
            responseData.Message = "Xin hãy thao tác sau 5 giây.";
            return Json(responseData);

        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "UsersController.UpdateStatus", Description = "Tài khoản - Đóng/Mở tài khoản")]
        public ActionResult UpdateStatus(int userId)
        {
            ResponseData responseData = new ResponseData();
            UserOnList usersModel = _userBo.GetById(userId);
            if (usersModel != null)
            {

                int status = usersModel.Status == (int)UserStatusAdmin.Actived
                    ? (int)UserStatusAdmin.Deactived
                    : (int)UserStatusAdmin.Actived;

                usersModel.Status = status;
                ErrorCodes errorCodes = _userBo.Update(usersModel);

                responseData.Success = errorCodes == ErrorCodes.Success;
                responseData.Message = StringUtils.GetEnumDescription(errorCodes);
                return Json(responseData);
            }


            responseData.Success = false;
            responseData.Message = "";
            return Json(responseData);

        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "UsersController.UpdateStatus", Description = "Tài khoản - Reset mật khẩu")]
        public ActionResult ResetPassWord(int userId, string passWord)
        {
            ResponseData responseData = new ResponseData();
            UserOnList usersModel = _userBo.GetById(userId);
            if (usersModel != null && !string.IsNullOrEmpty(passWord))
            {

                usersModel.Password = Crypton.Encrypt(passWord);
                ErrorCodes errorCodes = _userBo.Update(usersModel);

                responseData.Success = errorCodes == ErrorCodes.Success;
                responseData.Message = StringUtils.GetEnumDescription(errorCodes);
                return Json(responseData);
            }


            responseData.Success = false;
            responseData.Message = "";
            return Json(responseData);

        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "UsersController.ClearCacheAuthen", Description = "Tài khoản - Xóa cache phân quyền người dùng")]
        public ActionResult ClearCacheAuthen(int userId)
        {
            ResponseData responseData = new ResponseData();
            responseData.Success = AuthenService.DeleteAuthGroupActionMappingOnCache(userId);
            return Json(responseData);
        }

        [HttpPost]
        public ActionResult GeneratePassWord()
        {
            ResponseData responseData = new ResponseData();
            string strPass = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);

            responseData.Data = strPass;
            responseData.Success = true;
            responseData.Message = "";
            return Json(responseData);
        }
        [HttpPost]
        public JsonResult GetListUserType()
        {
            ResponseData responseData = new ResponseData();
            var usersModel = new UsersModel();

            //usersModel.UsersTypeList = Utilities.Utils.EnumToSelectListDes<UserTypeEnum>().ToList();

            var lst = _authGroupBo.GetAll();
            if (lst != null && lst.Any())
            {
                lst = lst.Where(x => x.Status == 1);
                usersModel.UsersTypeList = lst.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();
            }

            responseData.Data = usersModel;
            responseData.Success = true;
            return Json(responseData);
        }
        [HttpPost]
        public JsonResult GetListAuthGroup()
        {
            ResponseData responseData = new ResponseData();
            var lst = _authGroupBo.GetAll();
            if (lst != null && lst.Any())
            {
                lst = lst.Where(x => x.Status == 1);
                responseData.Data = lst;
                responseData.Success = true;
            }
            return Json(responseData);
        }
        public JsonResult GenerateQR()
        {
            ResponseData responseData = new ResponseData();
            var gga = new GoogleTOTP();
            if (!gga.GenerateImage())
            {
                responseData.Success = false;
                responseData.Message = "Generate QR fail!";
            }
            else
            {
                responseData.Success = true;
                responseData.Message = "Generate QR successed!";
            }

            return Json(responseData, JsonRequestBehavior.AllowGet);
        }
        #region private
        private void UpdateAuthGroupUserMapping(int userId, int authGroupId)
        {
            if (userId > 0 && authGroupId > 0)
            {
                string userName = AuthenService.GetUserLogin().UserName;
                var errorCodeAuthGroupMapping = _authGroupUserMappingBo.DeleteByGroupId(userId);
                if (errorCodeAuthGroupMapping == ErrorCodes.Success)
                {
                    _authGroupUserMappingBo.Insert(new AuthGroupUserMapping
                    {
                        AuthGroupId = authGroupId,
                        UserId = userId,
                        CreatedBy = userName
                    });
                }
            }
        }
        #endregion
    }
}