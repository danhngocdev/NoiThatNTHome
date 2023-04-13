using DVG.WIS.PublicModel.CMS;
using DVG.WIS.Utilities;
using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DVG.WIS.Core.Constants;
using DVG.WIS.Business.Galleries;

namespace DVG.CMS.Controllers
{
    public class GalleryController : Controller
    {
        private IGalleryBo _galleryBo;
        public GalleryController(IGalleryBo GalleryBo)
        {
            _galleryBo = GalleryBo;
        }

        // GET: Gallery
        [IsValidUrlRequest(KeyName = "GalleryController.Index", Description = "Gallery - Danh sách")]
        public ActionResult Index(int? bannerId)
        {
            GallerySearchModel model = new GallerySearchModel();
            return View(model);
        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "GalleryController.Search", Description = "Gallery - Danh sách")]
        public ActionResult Search(GallerySearchModel searchModel)
        {
            ResponseData responseData = new ResponseData();
            int totalRows = 0;
            var lstRet = _galleryBo.GetList(searchModel.Status,
                searchModel.PageIndex, searchModel.PageSize, out totalRows);
            if (null != lstRet)
            {
                searchModel.EditItem = new GalleryModel();
                searchModel.ListData = lstRet.Select(item => new GalleryModel(item)).ToList();
                responseData.Data = searchModel;
                responseData.TotalRow = totalRows;
                responseData.Success = true;
            }
            return Json(responseData);
        }
        [HttpPost]
        [IsValidUrlRequest(KeyName = "GalleryController.Search", Description = "Gallery - Cập nhật")]
        public ActionResult Update(GalleryModel model)
        {
            ResponseData responseData = new ResponseData();

            if (null != model)
            {
                Gallery Gallery = model.Id > 0 ? _galleryBo.GetById(model.Id) : new Gallery();
                Gallery.Url = model.Url.Replace(StaticVariable.DomainImage, string.Empty).TrimStart('/');
                Gallery.Status = model.Status;
                Gallery.Priority = model.Priority;
                //Update banner
                ErrorCodes result = _galleryBo.Update(Gallery);
                responseData.Success = result == ErrorCodes.Success;
                responseData.Message = StringUtils.GetEnumDescription(result);
            }
            return Json(responseData);
        }
        [HttpPost]
        [IsValidUrlRequest(KeyName = "GalleryController.Delete", Description = "Gallery - Xóa")]
        public ActionResult Delete(int id)
        {
            ResponseData responseData = new ResponseData();
            ErrorCodes result = _galleryBo.Delete(id);
            responseData.Success = result == ErrorCodes.Success;
            responseData.Message = StringUtils.GetEnumDescription(result);
            return Json(responseData);
        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "GalleryController.GetGallery", Description = "Gallery - Xem chi tiết")]
        public ActionResult GetGallery(int bannerId)
        {
            ResponseData responseData = new ResponseData();

            responseData.Success = true;
            responseData.Data = new GalleryModel(_galleryBo.GetById(bannerId));

            return Json(responseData);
        }
    }
}