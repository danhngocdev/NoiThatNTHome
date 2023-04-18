using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DVG.Website.Controllers
{
    public class SiteMapController : BaseController
    {
        // GET: SiteMap
        public ActionResult Index()
        {
            return View();
        }
    }
}