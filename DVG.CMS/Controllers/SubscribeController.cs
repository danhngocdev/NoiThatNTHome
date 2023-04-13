using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DVG.WIS.Business.Category;
using DVG.WIS.Business.Subscribe;
using DVG.WIS.Core.Enums;
using DVG.WIS.Entities;
using DVG.WIS.PublicModel.CMS;
using DVG.WIS.Utilities;

namespace DVG.CMS.Controllers
{
    public class SubscribeController : Controller
    {
        private ISubscribeBo _subscribeBo;
        private List<EnumHelper.Enums> ListAllSubscribeStatus;

        public SubscribeController(ISubscribeBo subscribeBo)
        {
            _subscribeBo = subscribeBo;
            this.ListAllSubscribeStatus = EnumHelper.Instance.ConvertEnumToList<SubscribeStatusEnum>().ToList();
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Search(SubscribeSearchModel searchModel)
        {
            ResponseData responseData = new ResponseData();
            int totalRows = 0;
            var lstRet = _subscribeBo.GetList(searchModel.Email, searchModel.PageIndex, searchModel.PageSize, out totalRows);
            if (null != lstRet)
            {
                searchModel.ListData = lstRet.Select(item => new SubscribeModel(item)).ToList();
                foreach (var item in searchModel.ListData)
                {
                    if (item.Status > 0)
                        item.StatusName = ListAllSubscribeStatus.Find(x => x.Id == item.Status).Name;
                }
                responseData.Data = searchModel.ListData;
                responseData.TotalRow = totalRows;
                responseData.Success = true;
            }
            return Json(responseData);
        }

        public ActionResult Delete(int id)
        {
            ResponseData responseData = new ResponseData();
            ErrorCodes result = _subscribeBo.Delete(id);
            responseData.Success = result == ErrorCodes.Success;
            responseData.Message = StringUtils.GetEnumDescription(result);
            return Json(responseData);
        }
        public ActionResult UpdateStatus(int id, string action)
        {
            ResponseData responseData = new ResponseData();
            ErrorCodes result = _subscribeBo.UpdateStatus(id, action);
            responseData.Success = result == ErrorCodes.Success;
            responseData.Message = StringUtils.GetEnumDescription(result);
            return Json(responseData);
        }
    }
}