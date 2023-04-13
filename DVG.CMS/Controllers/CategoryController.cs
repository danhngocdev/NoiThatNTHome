using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DVG.WIS.Business.Category;
using DVG.WIS.Utilities;
using DVG.WIS.Entities;
using DVG.WIS.Utilities.SignalrHelper;
using DVG.WIS.Business.Authenticator;
using DVG.WIS.Core.Enums;
using DVG.CMS.Models;

namespace DVG.CMS.Controllers
{
	public class CategoryController : Controller
	{
		private ICategoryBo _categoryBo;

		public CategoryController(ICategoryBo categoryBo)
		{
			this._categoryBo = categoryBo;
		}

        [IsValidUrlRequest(KeyName = "CategoryController.Index", Description = "Chuyên mục - Danh sách")]
        public ActionResult Index()
		{
			CategoryParamModel paramModel = new CategoryParamModel();
			paramModel.ListCateParrent = _categoryBo.GetByStatus(0).ToList();
			paramModel.ListNewsType = EnumHelper.Instance.ConvertEnumToList<CategoryTypeEnum>().ToList();
			return View(paramModel);
		}

		[HttpPost]
        [IsValidUrlRequest(KeyName = "CategoryController.Search", Description = "Chuyên mục - Danh sách")]
        public ActionResult Search(CategorySearchModel searchModel)
		{
			ResponseData responseData = new ResponseData();
			List<CategoryModelCMS> models = new List<CategoryModelCMS>();
			int totalRow = 0;
			var listCategory = _categoryBo.GetListPaging(searchModel.Keyword, searchModel.ParentId, searchModel.NewsType, searchModel.PageIndex, searchModel.PageSize, out totalRow);
			if (null != listCategory)
			{
				models = listCategory.Select(item => new CategoryModelCMS(item)).ToList();
				var lstStatusName = EnumHelper.Instance.ConvertEnumToList<CategoryState.CategoryStatusEnum>().ToList();
				foreach (var item in models)
				{
                    EnumHelper.Enums newsTypeEnums = item.ListNewsType.ToList().Find(x => x.Id == item.Type);
                    EnumHelper.Enums newsStatusEnums = lstStatusName.Find(x => x.Id == item.Status);
                    if (null != newsTypeEnums)
				    {
                        item.NewsTypeName = newsTypeEnums.Name;
                    }
				    if (null != newsStatusEnums)
				    {
                        item.StatusName = newsStatusEnums.Name;
                    }
				}
				responseData.Data = models;
				responseData.TotalRow = totalRow;
				responseData.Success = true;
			}
			return Json(responseData);
		}

        [HttpPost]
        [IsValidUrlRequest(KeyName = "CategoryController.UpdateCategory", Description = "Chuyên mục - Cập nhật")]
        public ActionResult UpdateCategory()
		{
			var encryptCateId = Request.QueryString["cateId"];
			ViewBag.Title = "Thêm mới chuyên mục";
			ViewBag.TitleAction = "Thêm mới";
			CategoryModelCMS model = new CategoryModelCMS();

			int cateId = string.IsNullOrEmpty(encryptCateId) ? 0 : EncryptUtility.DecryptIdToInt(encryptCateId);

			if (cateId > 0)
			{
                DVG.WIS.Entities.Category category = _categoryBo.GetById(Convert.ToInt32(cateId));
				model = new CategoryModelCMS(category);
				ViewBag.Title = "Cập nhật chuyên mục";
				ViewBag.TitleAction = "Cập nhật";
			}

			return View(model);
		}

        [HttpPost]
        public JsonResult GetByEncryptCateId(string encryptCateId)
        {
            var result = new ResponseData();
            int cateId = string.IsNullOrEmpty(encryptCateId) ? 0 : EncryptUtility.DecryptIdToInt(encryptCateId);
            CategoryModelCMS model = new CategoryModelCMS();
            if (cateId > 0)
            {
                var category = _categoryBo.GetById(Convert.ToInt32(cateId));
                model = new CategoryModelCMS(category);
            }
            result.Data = model;
            result.Success = true;
            return Json(result);
        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "CategoryController.Update", Description = "Chuyên mục - Cập nhật")]
        public ActionResult Update(Category categoryModel)
		{
			ResponseData responseData = new ResponseData();
			ErrorCodes errorCode = _categoryBo.Update(categoryModel);
			responseData.Success = errorCode == ErrorCodes.Success;
			responseData.Message = StringUtils.GetEnumDescription(errorCode);
			return Json(responseData);
		}

        [HttpPost]
        [IsValidUrlRequest(KeyName = "CategoryController.Delete", Description = "Chuyên mục - Xóa")]
        public ActionResult Delete(string encryptId)
		{
			ResponseData responseData = new ResponseData();
			int cateId = string.IsNullOrEmpty(encryptId) ? 0 : EncryptUtility.DecryptIdToInt(encryptId);
			var userInfo = AuthenService.GetUserLogin();
			if (userInfo != null)
			{
				ErrorCodes errorCode = _categoryBo.Delete(cateId, userInfo.UserName);
				responseData.Success = errorCode == ErrorCodes.Success;
				responseData.Message = StringUtils.GetEnumDescription(errorCode);
			}

			return Json(responseData);
		}

        [HttpPost]
        public ActionResult GetListCategory(int cateId)
		{
			ResponseData responseData = new ResponseData();
			var totalRow = 0;
			List<CategoryModelCMS> models = new List<CategoryModelCMS>();
			var listCategory = _categoryBo.GetListPaging("", 0, 0, 1, int.MaxValue, out totalRow);
			if (listCategory != null)
			{
				models = listCategory.Select(item => new CategoryModelCMS(item)).ToList();
				//Get list category
				foreach (var category in models)
				{
					category.Name = StringUtils.GetStringTreeview(category.Level) + category.Name;
				}
				responseData.Data = models;
				responseData.Success = true;
				responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.Success);
			}

			return Json(responseData);
		}

        [HttpPost]
        public ActionResult GetListCategoryByType(int newsType)
		{
			ResponseData responseData = new ResponseData();
			List<CategoryModelCMS> models = new List<CategoryModelCMS>();
			var listCategory = _categoryBo.GetListByType(newsType);
			if (listCategory != null)
			{
				models = listCategory.Select(item => new CategoryModelCMS(item)).ToList();
				//Get list category
				foreach (var category in models)
				{
					category.Name = StringUtils.GetStringTreeview(category.Level) + category.Name;
				}
				responseData.Data = models;
				responseData.Success = true;
				responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.Success);
			}
			return Json(responseData);
		}
	}
}
