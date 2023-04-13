using DVG.WIS.Business.Recruitments;
using DVG.WIS.Core;
using DVG.WIS.Entities;
using DVG.WIS.PublicModel.CMS;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DVG.CMS.Controllers
{
    public class RecruitmentController : Controller
    {
        private IRecruitmentBo _recruitmentBo;

        public RecruitmentController(IRecruitmentBo recruitmentBo)
        {
            _recruitmentBo = recruitmentBo;
        }

        [IsValidUrlRequest(KeyName = "RecruitmentController.Index", Description = "Recruitment - Danh sách")]
        public ActionResult Index(int? bannerId)
        {
            return View();
        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "RecruitmentController.Search", Description = "Recruitment - Danh sách")]
        public ActionResult Search(RecruitmentSearchModel searchModel)
        {
            ResponseData responseData = new ResponseData();
            int totalRows = 0;
            var lstRet = _recruitmentBo.GetList(searchModel.Position, searchModel.CateName, searchModel.Status,
                searchModel.PageIndex, searchModel.PageSize, out totalRows);
            if (null != lstRet)
            {
                searchModel.EditItem = new RecruitmentModel();
                searchModel.ListData = lstRet.Select(item => new RecruitmentModel(item)).ToList();
                responseData.Data = searchModel;
                responseData.TotalRow = totalRows;
                responseData.Success = true;
            }
            return Json(responseData);
        }
        [HttpPost]
        [IsValidUrlRequest(KeyName = "RecruitmentController.Search", Description = "Recruitment - Cập nhật")]
        public ActionResult Update(RecruitmentModel model)
        {
            ResponseData responseData = new ResponseData();

            if (null != model)
            {
                Recruitment banner = model.Id > 0 ? _recruitmentBo.GetById(model.Id) : new Recruitment();
                banner.CateName = model.CateName;
                banner.Status = model.Status;
                banner.Position = model.Position;
                banner.EndDate = model.EndDate;
                if (string.IsNullOrEmpty(model.EndDateStr))
                    banner.EndDate = null;
                else
                    banner.EndDate = Utils.ConvertStringToDateTime(model.EndDateStr, Const.NormalDateFormat);
                banner.Description = model.Description;
                banner.Address = model.Address;
                //Update banner
                ErrorCodes result = _recruitmentBo.Update(banner);
                responseData.Success = result == ErrorCodes.Success;
                responseData.Message = StringUtils.GetEnumDescription(result);
            }
            return Json(responseData);
        }
        [HttpPost]
        [IsValidUrlRequest(KeyName = "RecruitmentController.Delete", Description = "Recruitment - Xóa")]
        public ActionResult Delete(int id)
        {
            ResponseData responseData = new ResponseData();
            ErrorCodes result = _recruitmentBo.Delete(id);
            responseData.Success = result == ErrorCodes.Success;
            responseData.Message = StringUtils.GetEnumDescription(result);
            return Json(responseData);
        }
        [HttpPost]
        [IsValidUrlRequest(KeyName = "BannerController.Recruitment", Description = "Recruitment - Xem chi tiết")]
        public ActionResult GetRecruitment(int bannerId)
        {
            ResponseData responseData = new ResponseData();

            responseData.Success = true;
            responseData.Data = _recruitmentBo.GetById(bannerId);

            return Json(responseData);
        }
    }
}