using DVG.CMS;
using DVG.WIS.Business.PriceList;
using DVG.WIS.Core.Enums;
using DVG.WIS.Entities;
using DVG.WIS.PublicModel.CMS;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _2GE.CMS.Controllers
{
    public class PriceListController : Controller
    {
		private readonly IPriceListBo _priceListBo;

        public PriceListController(IPriceListBo priceListBo)
        {
            _priceListBo = priceListBo;
        }

        // GET: PriceList
        [IsValidUrlRequest(KeyName = "PriceListController.Index", Description = "Danh sách báo giá - Danh sách")]

        public ActionResult Index()
        {
            PriceListModelSearchModel PriceListSearchModel = new PriceListModelSearchModel()
            {
                ListStatus = EnumHelper.Instance.ConvertEnumToList<PriceListStatusEnum>(),
                ListUnit = EnumHelper.Instance.ConvertEnumToList<PriceListUnitEnum>()
            };
            return View(PriceListSearchModel);
        }




		[HttpPost]
		[IsValidUrlRequest(KeyName = "PriceListController.Search", Description = "Danh sách báo giá")]
		public ActionResult Search(PriceListModelSearchModel searchModel)
		{
			ResponseData responseData = new ResponseData();
			int totalRow = 0;
			var listPriceList = _priceListBo.GetList(searchModel.Keyword, searchModel.PageIndex, searchModel.PageSize,searchModel.Status, out totalRow);
			if (null != listPriceList)
			{
                searchModel.EditItem = new PriceListModel();
                searchModel.ListData = listPriceList.Select(item => new PriceListModel(item)).ToList();
                searchModel.ListStatus = EnumHelper.Instance.ConvertEnumToList<PriceListStatusEnum>();
                searchModel.ListUnit = EnumHelper.Instance.ConvertEnumToList<PriceListUnitEnum>();
                responseData.Data = searchModel;
                responseData.TotalRow = totalRow;
                responseData.Success = true;
            }
			return Json(responseData);
		}




        [HttpPost]
        public JsonResult GetPriceListById(int? Id)
        {
            var result = new ResponseData();
            PriceListModel model = new PriceListModel();
            if (Id > 0)
            {
                var pricelist = _priceListBo.GetById((int)Id);
                model = new PriceListModel(pricelist);
            }
            result.Data = model;
            result.Success = true;
            return Json(result);
        }


        [HttpPost]
        [IsValidUrlRequest(KeyName = "PriceListController.Update", Description = "Báo Giá - Cập nhật")]
        public ActionResult Update(PriceList priceListModel)
        {
            ResponseData responseData = new ResponseData();
            ErrorCodes errorCode = _priceListBo.Update(priceListModel);
            responseData.Success = errorCode == ErrorCodes.Success;
            responseData.Message = StringUtils.GetEnumDescription(errorCode);
            return Json(responseData);
        }


        [HttpPost]
        [IsValidUrlRequest(KeyName = "PriceListController.Delete", Description = "Báo Giá  - Xóa")]
        public ActionResult Delete(int id)
        {
            ResponseData responseData = new ResponseData();
            ErrorCodes result = _priceListBo.Delete(id);
            responseData.Success = result == ErrorCodes.Success;
            responseData.Message = StringUtils.GetEnumDescription(result);
            return Json(responseData);
        }


        //[HttpPost]
        //[IsValidUrlRequest(KeyName = "PriceListController.UpdatePricelist", Description = "Chuyên mục - Cập nhật")]
        //public ActionResult UpdatePricelist()
        //{
        //    //var encryptCateId = Request.QueryString["cateId"];
        //    //ViewBag.Title = "Thêm mới chuyên mục";
        //    //ViewBag.TitleAction = "Thêm mới";
        //    //CategoryModelCMS model = new CategoryModelCMS();

        //    //int cateId = string.IsNullOrEmpty(encryptCateId) ? 0 : EncryptUtility.DecryptIdToInt(encryptCateId);

        //    //if (cateId > 0)
        //    //{
        //    //    DVG.WIS.Entities.Category category = _categoryBo.GetById(Convert.ToInt32(cateId));
        //    //    model = new CategoryModelCMS(category);
        //    //    ViewBag.Title = "Cập nhật chuyên mục";
        //    //    ViewBag.TitleAction = "Cập nhật";
        //    //}

        //    //return View(model);
        //}

    }
}

