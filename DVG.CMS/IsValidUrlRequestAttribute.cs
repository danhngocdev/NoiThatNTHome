using DVG.WIS.Business.Authenticator;
using DVG.WIS.Business.AuthAction;
using DVG.WIS.Core;
using DVG.WIS.DAL.AuthAction;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DVG.CMS
{
    public class IsValidUrlRequestAttribute : AuthorizeAttribute
    {
        private IAuthActionBo _authActionBo;
        public IsValidUrlRequestAttribute()
        {
            _authActionBo = new AuthActionBo(new AuthActionDal());
        }
        public string KeyName { get; set; }
        public string Description { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        private List<string> ControlKeyHasPermession { get; set; }
        public bool NotAction { get; set; }
        private void CheckKeyNameInDb()
        {
            if (!string.IsNullOrEmpty(KeyName))
            {
                bool checkExist = false;
                if (!checkExist)
                {
                    var splits = KeyName.Split('.');
                    if (string.IsNullOrEmpty(Controller) && splits.Length > 0 && !string.IsNullOrEmpty(splits[0]))
                    {
                        Controller = splits[0];
                    }
                    if (string.IsNullOrEmpty(Action) && splits.Length > 1 && !string.IsNullOrEmpty(splits[1]))
                    {
                        Action = splits[1];
                    }
                    if (string.IsNullOrEmpty(Description))
                    {
                        Description = KeyName;
                    }
                    if (!string.IsNullOrEmpty(Controller) && !string.IsNullOrEmpty(Action))
                    {
                        AuthAction action = new AuthAction() { KeyName = KeyName, Description = Description, Controller = Controller, Action = Action, Status = 1 };
                        _authActionBo.Insert(action);
                    }
                }
            }
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (AuthenService.IsLogin())
            {
                if (!NotAction)
                {
                    CheckKeyNameInDb();
                    var user = AuthenService.GetUserLogin();
                    string userName = user.UserName;
                    string mobile = user.Mobile;

                    var isAdminAccount = userName.Equals(AppSettings.Instance.GetString(Const.GodAdminAccount));

                    // check phân quyền controller/action
                    if (isAdminAccount) return;

                    if (filterContext.ActionDescriptor != null && !string.IsNullOrEmpty(filterContext.ActionDescriptor.ActionName)
                        && filterContext.ActionDescriptor.ControllerDescriptor != null && filterContext.ActionDescriptor.ControllerDescriptor.ControllerType != null
                        && !string.IsNullOrEmpty(filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.Name))
                    {
                        var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.Name;
                        var actionName = filterContext.ActionDescriptor.ActionName;

                        var userPermission = AuthenService.GetAuthGroupActionMappingOnCache(user.UserId);

                        if (userPermission != null && userPermission.LstPermissionAction != null)
                        {
                            var valid = userPermission.LstPermissionAction.Any(x => x.Controller == controllerName && x.Action == actionName && x.Status == 1);
                            if (!valid)
                            {
                                RejectRequestNotPermission(filterContext);
                            }
                        }
                        else RejectRequestNotPermission(filterContext);
                    }
                    else RejectRequestNotPermission(filterContext);
                }
            }
            else
            {
                if (filterContext.HttpContext.Request.ContentType == "application/json;charset=utf-8" || filterContext.HttpContext.Request.ContentType == "application/json;charset=UTF-8")
                {
                    filterContext.Result = new JsonResult
                    {
                        Data = new
                        {
                            login = false,
                            Message = "Phiên đăng nhập đã hết hạn, vui lòng đăng nhập lại!"
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    HttpContext.Current.Response.Redirect("/dang-nhap?returnUrl=" + HttpContext.Current.Request.Url);
                }
            }

        }

        private void RejectRequestNotPermission(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.ContentType == "application/json;charset=utf-8" || filterContext.HttpContext.Request.ContentType == "application/json;charset=UTF-8")
            {
                ResponseData responseData = new ResponseData();
                responseData.Success = false;
                responseData.Message = "Bạn chưa được phân quyền chức năng này.";

                filterContext.Result = new JsonResult
                {
                    Data = responseData,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                HttpContext.Current.Response.Redirect("/permission-denied");
            }
        }
    }
}