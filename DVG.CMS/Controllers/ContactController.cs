using DVG.CMS;
using DVG.WIS.Business.InfoContact;
using DVG.WIS.Core.Enums;
using DVG.WIS.Entities;
using DVG.WIS.PublicModel;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _2GE.CMS.Controllers
{
    public class ContactController : Controller
    {
        private IInfoContactBo _infoContactBo;

        public ContactController(IInfoContactBo infoContactBo)
        {
            _infoContactBo = infoContactBo;
        }

        // GET: Contact
        [IsValidUrlRequest(KeyName = "ContactController.Index", Description = "Contact - Danh sách")]
        public ActionResult Index()
        {
            InfoContactSearchModel paramSearch = new InfoContactSearchModel();
            paramSearch.ListStatus = EnumHelper.Instance.ConvertEnumToList<InfoContactEnum>();
            return View(paramSearch);
        }


        [HttpPost]
        [IsValidUrlRequest(KeyName = "ContactController.Search", Description = "Danh sách khách hàng")]
        public ActionResult Search(InfoContactSearchModel searchModel)
        {
            ResponseData responseData = new ResponseData();
            
            int totalRow = 0;
            var listPriceList = _infoContactBo.GetList(searchModel.Keyword, searchModel.PageIndex, searchModel.PageSize, searchModel.Status, out totalRow);
            if (null != listPriceList)
            {
                //searchModel.EditItem = new PriceListModel();
                searchModel.ListData = listPriceList.Select(item => new InfoContactModel(item)).ToList();
                searchModel.ListStatus = EnumHelper.Instance.ConvertEnumToList<PriceListStatusEnum>();
                responseData.Data = searchModel;
                responseData.TotalRow = totalRow;
                responseData.Success = true;
            }
            return Json(responseData);
        }


        [HttpPost]
        [IsValidUrlRequest(KeyName = "ContactController.Update", Description = "Contact - Cập nhật")]
        public ActionResult Update(InfoContact contactModel)
        {
            ResponseData responseData = new ResponseData();
            ErrorCodes errorCode = _infoContactBo.Update(contactModel);
            responseData.Success = errorCode == ErrorCodes.Success;
            responseData.Message = StringUtils.GetEnumDescription(errorCode);
            return Json(responseData);
        }
    }
}