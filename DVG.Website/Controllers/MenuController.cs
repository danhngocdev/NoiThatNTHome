using DVG.WIS.Business.Menu;
using DVG.WIS.PublicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DVG.Website.Controllers
{
    public class MenuController : BaseController
    {
        private IMenuBo _menuBo;

        public MenuController(IMenuBo menuBo)
        {
            _menuBo = menuBo;
        }


        public ActionResult MenuTop()
        {
            var lstCate = _menuBo.GetListMenuTop();
            return PartialView("_MenuTop", lstCate);
        }
    }
}