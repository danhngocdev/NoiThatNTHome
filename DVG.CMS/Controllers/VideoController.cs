using DVG.WIS.Business.Authenticator;
using DVG.WIS.Business.Category;
using DVG.WIS.Business.News;
using DVG.WIS.Business.Users;
using DVG.WIS.Business.Video;
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
using DVG.WIS.PublicModel;
using static DVG.WIS.PublicModel.VideoModel;

namespace DVG.CMS.Controllers
{
    public class VideoController : Controller
    {
        private IUserBo _userBo;
        private readonly INewsBo _pageBo;
        private IVideoBo _videoBo;
        public VideoController(IUserBo userBo, INewsBo PageBo, IVideoBo videoBo)
        {
            _userBo = userBo;
            _pageBo = PageBo;
            _videoBo = videoBo;
        }

        //Video
        [IsValidUrlRequest(KeyName = "VideoController.Index", Description = "Danh sách video - Danh sách")]

        public ActionResult Index()
        {
            VideoModelSearchModel PriceListSearchModel = new VideoModelSearchModel()
            {
                ListStatus = EnumHelper.Instance.ConvertEnumToList<ListStatusEnum>(),
            };
            return View(PriceListSearchModel);
        }




        [HttpPost]
        [IsValidUrlRequest(KeyName = "VideoController.Search", Description = "Danh sách video")]
        public ActionResult Search(VideoModelSearchModel searchModel)
        {
            ResponseData responseData = new ResponseData();
            int totalRow = 0;
            var listPriceList = _videoBo.GetList(searchModel.Keyword, searchModel.PageIndex, searchModel.PageSize, searchModel.Status, out totalRow);
            if (null != listPriceList)
            {
                searchModel.EditItem = new VideoModel();
                searchModel.ListData = listPriceList.Select(item => new VideoModel(item)).ToList();
                if (searchModel.ListData !=null && searchModel.ListData.Any())
                {
                    foreach (var item in searchModel.ListData)
                    {
                        item.Avatar = $"https://img.youtube.com/vi/{item.VideoUrl}/0.jpg";
                    }
                }
                searchModel.ListStatus = EnumHelper.Instance.ConvertEnumToList<ListStatusEnum>();
                responseData.Data = searchModel;
                responseData.TotalRow = totalRow;
                responseData.Success = true;
            }
            return Json(responseData);
        }



        [HttpPost]
        public JsonResult GetVideoById(int? Id)
        {
            var result = new ResponseData();
            VideoModel model = new VideoModel();
            if (Id > 0)
            {
                var videoModel = _videoBo.GetById((int)Id);
                model = new VideoModel(videoModel);
            }
            result.Data = model;
            result.Success = true;
            return Json(result);
        }


        [HttpPost]
        [IsValidUrlRequest(KeyName = "VideoController.Update", Description = " Video- Cập nhật")]
        public ActionResult Update(Video video)
        {
            ResponseData responseData = new ResponseData();
            ErrorCodes errorCode = _videoBo.Update(video);
            responseData.Success = errorCode == ErrorCodes.Success;
            responseData.Message = StringUtils.GetEnumDescription(errorCode);
            return Json(responseData);
        }


        [HttpPost]
        [IsValidUrlRequest(KeyName = "VideoController.Delete", Description = "Video  - Xóa")]
        public ActionResult Delete(int id)
        {
            ResponseData responseData = new ResponseData();
            ErrorCodes result = _videoBo.Delete(id);
            responseData.Success = result == ErrorCodes.Success;
            responseData.Message = StringUtils.GetEnumDescription(result);
            return Json(responseData);
        }





        [IsValidUrlRequest(KeyName = "VideoController.UpdateVideo", Description = "Video - Cập nhật")]
        public ActionResult UpdateVideo(string videoId = "")
        {
            string encript = Request.QueryString["videoId"];
            int id = !string.IsNullOrEmpty(encript) ? EncryptUtility.DecryptIdToInt(videoId) : 0;
            //string userName = AuthenService.GetUserLogin().UserName;
            //var statusOfPagePermission = AuthenService.ProcessPermissionStatusOfPage(userId);
            //ViewBag.StatusOfPagePermission = statusOfPagePermission;
            VideoModel videoModel = new VideoModel();
            if (id != 0)
            {
                var video = _pageBo.GetVideoById(id);
                videoModel = new VideoModel(video);
                ViewBag.Title = "Cập nhật video";
                ViewBag.TitleAction = "Cập nhật";
                ViewBag.Idstr = videoModel.EncryptPageId;
            }
            else
            {
                ViewBag.Idstr = string.Empty;
            }
            return View(videoModel);
        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "VideoController.UpdateVideo", Description = "Video - Cập nhật")]
        public ActionResult UpdateVideo(VideoModel video, int currentStatus)
        {
            ResponseData responseData = this.UpdateVideoProcess(video, currentStatus);
            return Json(responseData);
        }

        #region private

        private ResponseData UpdateVideoProcess(VideoModel model, int currentStatus)
        {
            ResponseData responseData = new ResponseData();
            if (!AuthenService.IsLogin())
            {
                responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.NotLogin);
                return responseData;
            }

            Video video = new Video();

            if (model != null && model.Id > 0)
            {
                video = _pageBo.GetVideoById(model.Id);
            }

            if (video != null)
            {

                string userName = AuthenService.GetUserLogin().UserName;
                video.Title = model.Title;
                video.VideoUrl = model.VideoUrl;
                video.Link = model.Link;
                video.Avatar = model.Avatar;

                ErrorCodes errorCode = _pageBo.UpdateVideo(video);
                responseData.Success = errorCode == ErrorCodes.Success;
                responseData.Message = StringUtils.GetEnumDescription(errorCode);
            }
            return responseData;
        }

        #endregion
    }
}