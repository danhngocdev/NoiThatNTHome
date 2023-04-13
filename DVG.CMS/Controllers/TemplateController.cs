using DVG.WIS.Business.Authenticator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DVG.CMS.Controllers
{
    public class TemplateController : Controller
    {
        // GET: Template
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SidebarNav()
        {
            var userId = AuthenService.GetUserLogin().UserId;
            var permissionStatusOfNews = AuthenService.ProcessPermissionStatusOfNews(userId);

            return PartialView("~/Views/Shared/_SidebarNav.cshtml", permissionStatusOfNews);
        }

        public ActionResult Navbar()
        {
            return PartialView("~/Views/Shared/_Navbar.cshtml");
        }

        public ActionResult Footer()
        {
            return PartialView("~/Views/Shared/_Footer.cshtml");
        }
    }
}