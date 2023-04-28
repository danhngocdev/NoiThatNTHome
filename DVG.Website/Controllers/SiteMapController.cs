using DVG.WIS.Business.SiteMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DVG.Website.Controllers
{
    public class SiteMapController : BaseController
    {

        private readonly ISiteMapBo _siteMapBo;

        public SiteMapController(ISiteMapBo siteMapBo)
        {
            _siteMapBo = siteMapBo;
        }

        // GET: SiteMap
        public ActionResult Index()
        {
            string xml = _siteMapBo.GenSiteMapIndex();
            if (string.IsNullOrEmpty(xml))
                xml = string.Empty;
            return Content(xml, "text/xml");
        }

        public ActionResult SiteMapCategory()
        {
            string xml = _siteMapBo.GenSiteMapCategory();
            if (string.IsNullOrEmpty(xml))
                xml = string.Empty;
            return Content(xml, "text/xml");
        }

        public ActionResult SiteMapProduct()
        {
            string xml = _siteMapBo.GenSiteMapProduct();
            if (string.IsNullOrEmpty(xml))
                xml = string.Empty;
            return Content(xml, "text/xml");
        }

        public ActionResult SiteMapArticle()
        {
            string xml = _siteMapBo.GenSiteMapArticle();
            if (string.IsNullOrEmpty(xml))
                xml = string.Empty;
            return Content(xml, "text/xml");
        }

    }
}