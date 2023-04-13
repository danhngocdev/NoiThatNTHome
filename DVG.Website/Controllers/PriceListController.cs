using DVG.WIS.Business.PriceList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DVG.Website.Controllers
{
    public class PriceListController : BaseController
    {
        private readonly IPriceListBo _priceListBo;

        public PriceListController(IPriceListBo priceListBo)
        {
            _priceListBo = priceListBo;
        }

        // GET: PriceList
        public ActionResult Index()
        {
            return View();
        }
    }
}