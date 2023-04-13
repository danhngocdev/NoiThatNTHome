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

namespace DVG.Website.Controllers
{
    public class ContactController : BaseController
    {
        // GET: Contact
        private readonly IInfoContactBo _infoContactBo;

        public ContactController(IInfoContactBo infoContactBo)
        {
            _infoContactBo = infoContactBo;
        }

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult FormContact()
        {
            ContactFEModel form = new ContactFEModel();
            return PartialView("_FormContact", form);
        }

        [HttpPost]
        public JsonResult SendRequest(ContactFEModel contact)
        {
            ResponseData responseData = new ResponseData();
            var infoContact = new InfoContact();
            infoContact.Name = contact.Name;
            infoContact.Content = contact.Content;
            infoContact.CreatedDate = DateTime.Now;
            infoContact.Phone = contact.Phone;
            ErrorCodes errorCode = _infoContactBo.Update(infoContact);
            responseData.Success = errorCode == ErrorCodes.Success;
            responseData.Message = StringUtils.GetEnumDescription(errorCode);
            responseData.NextAction = (int)NextAction.ReloadPage;
            return Json(responseData);
            //return Json(responseData);
        }
    }
}