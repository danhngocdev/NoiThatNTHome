using DVG.CMS;
using DVG.WIS.Business.Category;
using DVG.WIS.Business.ProductShowHome;
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
    public class ProductShowHomeController : Controller
    {
        private readonly IProductShowHomeBo _productShowHomeBo;
        private ICategoryBo _categoryBo;
        public ProductShowHomeController(IProductShowHomeBo productShowHomeBo, ICategoryBo categoryBo)
        {
            _productShowHomeBo = productShowHomeBo;
            _categoryBo = categoryBo;
        }

        [IsValidUrlRequest(KeyName = "ProductShowHomeController.Index", Description = "Danh sách sản phẩm hiển thị - Danh sách")]

        public ActionResult Index()
        {
            ProductShowHomeSearchModel PriceListSearchModel = new ProductShowHomeSearchModel()
            {
                ListStatus = EnumHelper.Instance.ConvertEnumToList<ListStatusEnum>(),
            };
            return View(PriceListSearchModel);
        }


        [HttpPost]
        [IsValidUrlRequest(KeyName = "ProductShowHomeController.Search", Description = "Danh sách báo giá")]
        public ActionResult Search(ProductShowHomeSearchModel searchModel)
        {
            ResponseData responseData = new ResponseData();
            int totalRow = 0;
            var listProductShowHome = _productShowHomeBo.GetList(searchModel.Keyword, searchModel.PageIndex, searchModel.PageSize, searchModel.Status, out totalRow);
            if (null != listProductShowHome)
            {
                searchModel.EditItem = new ProductShowHomeModel();
                searchModel.ListData = listProductShowHome.Select(item => new ProductShowHomeModel(item)).ToList();
                if (searchModel.ListData != null && searchModel.ListData.Any())
                {
                    foreach (var item in searchModel.ListData)
                    {
                        item.CateName = _categoryBo.GetById(item.CategoryId)?.Name;
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
        public JsonResult GetProductShowHomeById(int? Id)
        {
            var result = new ResponseData();
            ProductShowHomeModel model = new ProductShowHomeModel();
            if (Id > 0)
            {
                var productShowHome = _productShowHomeBo.GetById((int)Id);
                model = new ProductShowHomeModel(productShowHome);
            }
            result.Data = model;
            result.Success = true;
            return Json(result);
        }


        [HttpPost]
        [IsValidUrlRequest(KeyName = "ProductShowHomeController.Update", Description = "Cập nhật")]
        public ActionResult Update(ProductShowHome productShowHome)
        {
            ResponseData responseData = new ResponseData();
            ErrorCodes errorCode = _productShowHomeBo.Update(productShowHome);
            responseData.Success = errorCode == ErrorCodes.Success;
            responseData.Message = StringUtils.GetEnumDescription(errorCode);
            return Json(responseData);
        }


        [HttpPost]
        [IsValidUrlRequest(KeyName = "ProductShowHomeController.Delete", Description = "Xóa")]
        public ActionResult Delete(int id)
        {
            ResponseData responseData = new ResponseData();
            ErrorCodes result = _productShowHomeBo.Delete(id);
            responseData.Success = result == ErrorCodes.Success;
            responseData.Message = StringUtils.GetEnumDescription(result);
            return Json(responseData);
        }
    }
}