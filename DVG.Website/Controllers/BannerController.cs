using DVG.WIS.Business.Banner;
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
    public class BannerController : Controller
    {
        private IBannerBo _bannerBo;

        public BannerController(IBannerBo bannerBo)
        {
            _bannerBo = bannerBo;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HeadBanner(int pageId)
        {
            int platform = BannerPlatformEnum.Web.GetHashCode();
            if (Request.Browser.IsMobileDevice)
                platform = BannerPlatformEnum.Wap.GetHashCode();
            var lstBanner = _bannerBo.GetBannerByCondition(pageId, BannerPositionEnum.Main.GetHashCode(), platform);
            List<BannerFEModel> lstBannerModel = new List<BannerFEModel>();
            if (lstBanner != null && lstBanner.Any())
            {
                lstBannerModel = lstBanner.Select(x => new BannerFEModel(x)).ToList();
            }
            return PartialView("_HeadBanner", lstBannerModel);
        }


        public ActionResult BannerHomePage(int pageId)
        {
            int platform = BannerPlatformEnum.Web.GetHashCode();
            if (Request.Browser.IsMobileDevice)
                platform = BannerPlatformEnum.Wap.GetHashCode();
            var lstBanner = _bannerBo.GetBannerByCondition(pageId, BannerPositionEnum.Main.GetHashCode(), platform);
            List<BannerFEModel> lstBannerModel = new List<BannerFEModel>();
            if (lstBanner != null && lstBanner.Any())
            {
                lstBannerModel = lstBanner.Select(x => new BannerFEModel(x)).ToList();
            }
            return PartialView("_BannerHomePage", lstBannerModel);
        }

        public ActionResult BannerByPosition(int pageId, int positionId)
        {
            int platform = BannerPlatformEnum.Web.GetHashCode();
            if (Request.Browser.IsMobileDevice)
                platform = BannerPlatformEnum.Wap.GetHashCode();
            var lstBanner = _bannerBo.GetBannerByCondition(pageId, positionId, platform);
            List<BannerFEModel> lstBannerModel = new List<BannerFEModel>();
            if (lstBanner != null && lstBanner.Any())
            {
                lstBannerModel = lstBanner.Select(x => new BannerFEModel(x)).ToList();
            }
            return PartialView("_BannerByPosition", lstBannerModel);
        }
    }
}