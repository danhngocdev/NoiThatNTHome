using DVG.WIS.Business.Customers;
using DVG.WIS.Core;
using DVG.WIS.Core.Enums;
using DVG.WIS.Entities;
using DVG.WIS.Local;
using DVG.WIS.PublicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DVG.Website.Controllers
{
    public class CustomerController : Controller
    {
        private ICustomerBo _customerBo;

        public CustomerController(ICustomerBo customerBo)
        {
            _customerBo = customerBo;
        }

        public ActionResult ContactPopup()
        {
            CustomerFEModel model = new CustomerFEModel();
            return PartialView("_ContactPopup");
        }


        // GET: Customer
        [HttpPost]
        public ActionResult Register(CustomerFEModel model)
        {
            Message objMsg = new Message();
            if (ModelState.IsValid)
            {
                Customer customer = new Customer()
                {
                    Name = model.Name,
                    Address = model.Address,
                    Phone = model.Phone,
                    Email = model.Email,
                    Title = !string.IsNullOrEmpty(model.Title) ? model.Title : string.Empty,
                    Description = !string.IsNullOrEmpty(model.Description) ? model.Description : string.Empty,
                    CreatedDate = DateTime.Now
                };
                ErrorCodes result = _customerBo.Update(customer);
                if (result == ErrorCodes.Success)
                {
                    objMsg.Error = false;
                    objMsg.Title = Resource.ContactSucess;
                    objMsg.NextAction = (int)NextAction.ReloadPage;
                }
                else
                {
                    objMsg.Error = true;
                    objMsg.Title = Notify.SystemError;
                }
            }
            else
            {
                objMsg.Error = true;
                objMsg.Title = string.Join("</br>", ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage));
            }
            return Json(objMsg);
        }
       
    }
}