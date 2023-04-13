using DVG.WIS.Business.Authenticator;
using DVG.WIS.Business.Category;
using DVG.WIS.Business.News;
using DVG.WIS.Business.Users;
using DVG.WIS.Core;
using DVG.WIS.Core.Constants;
using DVG.WIS.Core.Enums;
using DVG.WIS.Entities;
using DVG.WIS.PublicModel.CMS;
using DVG.WIS.Utilities;
using DVG.WIS.Utilities.FileStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DVG.CMS.Controllers
{
    public class PageController : Controller
    {
        private IUserBo _userBo;
        private readonly INewsBo _pageBo;

        public PageController(IUserBo userBo, INewsBo PageBo)
        {
            _userBo = userBo;
            _pageBo = PageBo;
        }


        [IsValidUrlRequest(KeyName = "PageController.UpdatePage", Description = "Page - Cập nhật")]
        public ActionResult UpdatePage(string pageId = "")
        {
            string encript = Request.QueryString["PageId"];
            int id = !string.IsNullOrEmpty(encript) ? EncryptUtility.DecryptIdToInt(pageId) : 0;
            //string userName = AuthenService.GetUserLogin().UserName;
            //var statusOfPagePermission = AuthenService.ProcessPermissionStatusOfPage(userId);
            //ViewBag.StatusOfPagePermission = statusOfPagePermission;
            PageModel PageModel = new PageModel();
            if (id != 0)
            {
                var news = _pageBo.GetPageById(id);
                PageModel = new PageModel(news);
                ViewBag.Title = "Cập nhật tin";
                ViewBag.TitleAction = "Cập nhật";
                ViewBag.Idstr = PageModel.EncryptPageId;
            }
            else
            {
                ViewBag.Idstr = string.Empty;
            }
            return View(PageModel);
        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "PageController.UpdatePage", Description = "Page - Cập nhật")]
        public ActionResult UpdatePage(PageModel Page, int currentStatus)
        {
            ResponseData responseData = this.UpdatePageProcess(Page, currentStatus);
            return Json(responseData);
        }

        #region private

        private ResponseData UpdatePageProcess(PageModel Page, int currentStatus)
        {
            ResponseData responseData = new ResponseData();
            if (!AuthenService.IsLogin())
            {
                responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.NotLogin);
                return responseData;
            }

            Page PageInfo = new Page();

            if (Page != null && Page.Id > 0)
            {
                PageInfo = _pageBo.GetPageById(Page.Id);
            }

            if (Page != null)
            {

                string userName = AuthenService.GetUserLogin().UserName;
                PageInfo.Title = Page.Title;
                PageInfo.Description = Page.Description;
                PageInfo.SEOTitle = Page.SEOTitle;
                PageInfo.SEODescription = Page.SEODescription;
                PageInfo.SEOKeyword = Page.SEOKeyword;
                try
                {
                    string avatar = string.Empty;
                    PageInfo.Description = StringUtils.UploadImageIncontent(Page.Description, out avatar, StaticVariable.ImageRootPath, string.Empty, StaticVariable.DomainImage, FileStorage.DownloadImageFromURL);
                    PageInfo.Description = Regex.Replace(PageInfo.Description, @"(<p([^>]+)?>([\s\r\n]+)?<(strong|b)>([\s\r\n]+)?Xem thêm(\s+)?:([\s\r\n]+)?</(strong|b)>([\s\r\n]+)?</p>.+)", string.Empty, RegexOptions.Singleline);
                }
                catch (Exception ex)
                {
                    PageInfo.Description = Page.Description;
                    Logger.ErrorLog(ex);
                }
                ErrorCodes errorCode = _pageBo.UpdatePage(PageInfo);
                responseData.Success = errorCode == ErrorCodes.Success;
                responseData.Message = StringUtils.GetEnumDescription(errorCode);
            }
            return responseData;
        }

        #endregion
    }
}