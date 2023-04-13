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
    public class NewsController : Controller
    {
        private IUserBo _userBo;
        private readonly INewsBo _newsBo;
        private readonly ICategoryBo _categoryBo;

        public NewsController(IUserBo userBo, INewsBo newsBo, ICategoryBo categoryBo)
        {
            _userBo = userBo;
            _newsBo = newsBo;
            _categoryBo = categoryBo;
        }

        [IsValidUrlRequest(KeyName = "NewsController.Index", Description = "News - Danh sách")]
        public ActionResult Index()
        {
            var userId = AuthenService.GetUserLogin().UserId;
            var statusOfNewsPermission = AuthenService.ProcessPermissionStatusOfNews(userId);
            NewsSearchModel newsSearchModel = new NewsSearchModel()
            {
                ListStatus = EnumHelper.Instance.ConvertEnumToList<NewsStatusEnum>(),
                StatusOfNewsPermission = statusOfNewsPermission
            };
            return View(newsSearchModel);
        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "NewsController.NewsView", Description = "Tin tức - Xem chi tiết")]
        public ActionResult NewsView(string newsId)
        {
            ResponseData responseData = new ResponseData();

            int id = !string.IsNullOrEmpty(newsId) ? EncryptUtility.DecryptIdToInt(newsId) : 0;

            News news = _newsBo.GetById(id);

            if (null != news)
            {
                NewsModel newsModel = new NewsModel(news);
                responseData.Data = newsModel;
                responseData.Success = true;

            }
            return Json(responseData);
        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "CustomerController.Search", Description = "Customer - Danh sách")]
        public ActionResult Search(NewsSearchModel searchModel)
        {
            ResponseData responseData = new ResponseData();
            int totalRows = 0;
            var lstRet = _newsBo.GetList(searchModel.CategoryId, searchModel.Status, searchModel.Keyword,
                searchModel.PageIndex, searchModel.PageSize, out totalRows);
            if (null != lstRet)
            {
                searchModel.EditItem = new NewsModel();
                searchModel.ListData = lstRet.Select(item => new NewsModel(item)).ToList();
                responseData.Data = searchModel;
                responseData.TotalRow = totalRows;
                responseData.Success = true;
            }
            return Json(responseData);
        }


        [IsValidUrlRequest(KeyName = "NewsController.UpdateNews", Description = "Tin tức - Cập nhật")]
        public ActionResult UpdateNews(int? newsType, string newsId = "", bool autosave = false)
        {
            string encript = Request.QueryString["newsId"];
            int id = !string.IsNullOrEmpty(encript) ? EncryptUtility.DecryptIdToInt(newsId) : 0;
            string userName = AuthenService.GetUserLogin().UserName;
            ViewBag.PendingApproved = (int)NewsStatusEnum.PendingApproved;
            ViewBag.Published = (int)NewsStatusEnum.Published;
            ViewBag.UnPublished = (int)NewsStatusEnum.UnPublished;
            ViewBag.Deleted = (int)NewsStatusEnum.Deleted;
            ViewBag.Status = 0;
            ViewBag.Title = "Thêm mới tin";
            ViewBag.TitleAction = "Thêm mới";
            var userId = AuthenService.GetUserLogin().UserId;
            //var statusOfNewsPermission = AuthenService.ProcessPermissionStatusOfNews(userId);
            //ViewBag.StatusOfNewsPermission = statusOfNewsPermission;
            NewsModel newsModel = new NewsModel();
            if (id != 0)
            {
                var news = _newsBo.GetById(id);
                // check phân quyền theo trạng thái tin
                AuthenService.CheckPermissionNewsStatus(news.Status, isAjax: false);
                var newsDetail = _newsBo.GetById(id);
                newsModel = new NewsModel(news);
                newsModel.ListImage = _newsBo.GetListImageByNewsId(id).ToList();
                ViewBag.Title = "Cập nhật tin";
                ViewBag.TitleAction = "Cập nhật";
                ViewBag.Status = news.Status;
                ViewBag.Idstr = newsModel.EncryptNewsId;

            }
            else
            {
                newsModel.CreatedBy = userName;
                newsModel.PublishedDate = DateTime.Now;
                newsModel.Type = newsType == null ? (int)NewsTypeEnum.News : newsType.Value;
                ViewBag.Idstr = string.Empty;
            }
            newsModel.ListLanguage = EnumHelper.Instance.ConvertEnumToList<LanguageEnum>();
            return View(newsModel);
        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "NewsController.UpdateNews", Description = "Tin tức - Cập nhật")]
        public ActionResult UpdateNews(NewsModel news, int currentStatus)
        {
            ResponseData responseData = this.UpdateNewsProcess(news, currentStatus);
            return Json(responseData);
        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "NewsController.DoPendingApprove", Description = "Tin tức - Gửi chờ duyệt")]
        public ActionResult DoPendingApprove(string newsId)
        {
            ResponseData responseData = this.UpdateStatus(newsId, (int)NewsStatusEnum.PendingApproved);
            return Json(responseData);
        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "NewsController.DoPublish", Description = "Tin tức - Xuất bản")]
        public ActionResult DoPublish(string newsId)
        {
            ResponseData responseData = this.UpdateStatus(newsId, (int)NewsStatusEnum.Published);
            return Json(responseData);
        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "NewsController.DoUnPublish", Description = "Tin tức - Gỡ bài")]
        public ActionResult DoUnPublish(string newsId)
        {
            ResponseData responseData = this.UpdateStatus(newsId, (int)NewsStatusEnum.UnPublished);
            return Json(responseData);
        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "NewsController.DoDelete", Description = "Tin tức - Xóa bài")]
        public ActionResult DoDelete(string newsId)
        {
            ResponseData responseData = this.UpdateStatus(newsId, (int)NewsStatusEnum.Deleted);
            return Json(responseData);
        }

        #region private

        private ResponseData UpdateNewsProcess(NewsModel news, int currentStatus)
        {
            ResponseData responseData = new ResponseData();
            if (!AuthenService.IsLogin())
            {
                responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.NotLogin);
                return responseData;
            }

            News newsInfo = new News();
            if (news != null && news.Id > 0)
            {
                newsInfo = _newsBo.GetById(news.Id);
            }

            if (news != null)
            {
                if (news.Id > 0)
                {
                    // check phân quyền theo trạng thái tin
                    AuthenService.CheckPermissionNewsStatus(news.Status);
                    AuthenService.CheckPermissionNewsStatus(currentStatus);
                }

                string userName = AuthenService.GetUserLogin().UserName;
                newsInfo.CreatedBy = userName;
                newsInfo.ModifiedBy = userName;
                // nếu bài chuyển trạng thái xuất bản và PulishDate < DateTime.Now thì gán bằng now, còn không thì giữ nguyên
                if (currentStatus != (int)NewsStatusEnum.Published && news.Status == (int)NewsStatusEnum.Published && news.PublishedDate < DateTime.Now)
                {
                    newsInfo.PublishedDate = DateTime.Now;
                }
                else
                    newsInfo.PublishedDate = Utils.ConvertStringToDateTime(news.PublishedDateStr, Const.CustomeDateFormat);

                newsInfo.Title = news.Title;
                newsInfo.Sapo = news.Sapo;
                newsInfo.Avatar = news.Avatar.Replace(StaticVariable.DomainImage.TrimEnd('/'), string.Empty).Trim('/');
                newsInfo.Description = news.Description;
                newsInfo.CategoryId = news.CategoryId;
                newsInfo.Status = news.Status;
                newsInfo.Source = news.Source;
                newsInfo.IsHighLight = news.IsHighLight;
                newsInfo.TextSearch = string.Format("{0} {1} {2}", newsInfo.Title, newsInfo.Sapo, newsInfo.Description);
                try
                {
                    string avatar = string.Empty;
                    newsInfo.Description = StringUtils.UploadImageIncontent(news.Description, out avatar, StaticVariable.ImageRootPath, string.Empty, StaticVariable.DomainImage, FileStorage.DownloadImageFromURL);
                    newsInfo.Description = Regex.Replace(newsInfo.Description, @"(<p([^>]+)?>([\s\r\n]+)?<(strong|b)>([\s\r\n]+)?Xem thêm(\s+)?:([\s\r\n]+)?</(strong|b)>([\s\r\n]+)?</p>.+)", string.Empty, RegexOptions.Singleline);
                }
                catch (Exception ex)
                {
                    newsInfo.Description = news.Description;
                    Logger.ErrorLog(ex);
                }
                newsInfo.SEODescription = news.SEODescription;
                newsInfo.SEOTitle = news.SEOTitle;
                newsInfo.SEOKeyword = news.SEOKeyword;
                newsInfo.LanguageId = 0;
                ErrorCodes errorCode = _newsBo.Update(newsInfo, news.ListImage);
                responseData.Success = errorCode == ErrorCodes.Success;
                responseData.Message = StringUtils.GetEnumDescription(errorCode);
            }
            return responseData;
        }

        private ResponseData UpdateStatus(string newsId, int status)
        {
            ResponseData responseData = new ResponseData();
            if (AuthenService.IsLogin() && !string.IsNullOrEmpty(newsId))
            {
                int id = EncryptUtility.DecryptIdToInt(newsId);
                var newsInfo = _newsBo.GetById(id);
                if (newsInfo != null && newsInfo.Id > 0)
                {
                    // check phân quyền theo trạng thái tin
                    var res = AuthenService.CheckPermissionNewsStatus(newsInfo.Status);
                    if (!res.Success) return res;
                    res = AuthenService.CheckPermissionNewsStatus(status);
                    if (!res.Success) return res;

                    // nếu bài chuyển trạng thái xuất bản và PulishDate < DateTime.Now thì gán bằng now, còn không thì giữ nguyên
                    if (newsInfo.Status != (int)NewsStatusEnum.Published && status == (int)NewsStatusEnum.Published && newsInfo.PublishedDate < DateTime.Now)
                    {
                        newsInfo.PublishedDate = DateTime.Now;
                    }

                    string userName = AuthenService.GetUserLogin().UserName;
                    ErrorCodes errorCode = _newsBo.ChangeStatusNews(id, status, userName, newsInfo.PublishedDate);


                    responseData.ErrorCode = Utils.ConvertEnumToInt(errorCode);
                    responseData.Success = errorCode == ErrorCodes.Success;
                    responseData.Message = StringUtils.GetEnumDescription(errorCode);
                }
            }
            return responseData;
        }

        #endregion
    }
}