using DVG.CMS.Models;
using DVG.WIS.Business.Authenticator;
using DVG.WIS.Business.Users;
using DVG.WIS.Core;
using DVG.WIS.Core.Constants;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DVG.CMS.Controllers
{
    public class AccountController : Controller
    {
        private IUserBo _userBo;

        public AccountController(IUserBo userBo)
        {
            this._userBo = userBo;
        }

        public ActionResult Login(string returnUrl = "")
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "/Home";
            }
            if (AuthenService.IsLogin())
            {
                return Redirect(returnUrl);
            }

            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = Request.RawUrl;

            Session[Const.SessionCurrentUrl] = returnUrl;

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult DoLogin(string email, string password, bool isSavedPassword = false)
        {
            var loginResult = AuthenService.LoginForCms(email, password, isSavedPassword);
            return Json(new { status = loginResult.Success, message = loginResult.Message });
        }

        public ActionResult Logout()
        {
            string returnUrl = string.Empty;

            if (Request.UrlReferrer != null)
            {
                returnUrl = Request.UrlReferrer.ToString();
                if (returnUrl.Contains("LogOnSSO"))
                {
                    returnUrl = string.Empty;
                }
            }

            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = StaticVariable.CmsUrl;

            AuthenService.Logout();

            if (AppSettings.Instance.GetBool(Const.UsingSSO))
            {
                returnUrl = string.Format("{0}?returnUrl={1}", AppSettings.Instance.GetString(Const.SSOLogoutCallback),
                    HttpUtility.UrlEncode(returnUrl));
            }

            return Redirect(returnUrl);
        }

        [IsValidUrlRequest(KeyName = "AccountController.Manager", Description = "Tài khoản cá nhân - Đổi mật khẩu")]
        public ActionResult Manager()
        {
            return View();
        }


        [HttpPost]
        [IsValidUrlRequest(KeyName = "AccountController.ChangePassword", Description = "Tài khoản cá nhân - Đổi mật khẩu")]
        public JsonResult ChangePassword(ChangePasswordModel saveModel)
        {
            ResponseData responseData = new ResponseData();
            if (string.IsNullOrEmpty(saveModel.CurrentPassword))
            {
                responseData.Message = "Mật khẩu hiện tại không đúng";
            }
            else if (string.IsNullOrEmpty(saveModel.Password))
            {
                responseData.Message = "Mật khẩu không hợp lệ";
            }
            else if (string.IsNullOrEmpty(saveModel.ConfirmPassword))
            {
                responseData.Message = "Mật khẩu xác nhận không đúng";
            }
            else
            {
                var userInfo = AuthenService.GetUserLogin();
                var errors = _userBo.ChangePassword(userInfo.UserName, saveModel.CurrentPassword, saveModel.Password,
                    saveModel.ConfirmPassword);
                responseData.Message = StringUtils.GetEnumDescription(errors);
                if (errors == ErrorCodes.Success)
                {
                    responseData.Success = true;
                    AuthenService.Logout();
                    return Json(responseData);
                }
            }
            return Json(responseData);
        }

        public ActionResult GoogleCode()
        {
            return View();
        }

        [HttpPost]
        public JsonResult VerifyGoogleCode(string otp)
        {
            ResponseData responseData = new ResponseData();
            // google authen
            var gga = new DVG.WIS.Utilities.GoogleTOTP();
            if (string.IsNullOrEmpty(otp) || (!string.IsNullOrEmpty(otp) && !otp.ToString().ToLower().Equals(gga.GeneratePin())))
            {
                responseData.Success = false;
                responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.AccountWrongGoogleAuthenticator);
                responseData.ErrorCode = (int)ErrorCodes.AccountWrongGoogleAuthenticator;
            }
            else
            {
                responseData.Success = true;
            }
            return Json(responseData);
        }

        public ActionResult AccountInfo()
        {
            UserLogin userLogin = AuthenService.GetUserLogin();
            if (string.IsNullOrEmpty(userLogin.Avatar))
            {
                userLogin.Avatar = "/Content/Images/noavatar.png";
            }
            return View(userLogin);
        }

        [IsValidUrlRequest(NotAction = true)]
        public ActionResult PermissionDenied()
        {
            return View();
        }

        #region private
        private string GetGGQRBase64()
        {
            var url = GetGGQRUrl();
            var tuple = DVG.WIS.Utilities.FileStorage.FileStorage.SaveImage(url);
            if (tuple.Item1 != null)
            {
                byte[] imageBytes = tuple.Item1.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
            return null;
        }

        public string GetGGQRUrl()
        {
            return string.Format("{0}/{1}", StaticVariable.CmsUrlNoSlash, AppSettings.Instance.GetString(Const.GGQR).TrimStart('/'));
        }
        #endregion

    }
}