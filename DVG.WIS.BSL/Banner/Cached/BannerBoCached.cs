using DVG.WIS.Caching;
using DVG.WIS.Caching.Cached;
using DVG.WIS.Core;
using DVG.WIS.Core.Constants;
using DVG.WIS.Core.Enums;
using System.Collections.Generic;
using System.Linq;

namespace DVG.WIS.Business.Banner.Cached
{
    public class BannerBoCached : IBannerBoCached
    {
        private ICached _cacheClient;
        private IBannerBoFE _bannerBoFE;
        private int _longExpiredInMinute = StaticVariable.LongCacheTime;
        private int _shortExpiredInMinute = StaticVariable.ShortCacheTime;
        private int _mediumExpiredInMinute = StaticVariable.MediumCacheTime;

        private static List<Entities.Banner> staticBanners;

        public BannerBoCached(IBannerBoFE bannerBoFE, ICached cacheClient)
        {
            this._bannerBoFE = bannerBoFE;
            this._cacheClient = cacheClient;
        }

        public List<Entities.Banner> GetAllActive()
        {
            //if (staticBanners == null)
            //{
            string keyCached = KeyCacheHelper.GenCacheKey(ConstKeyCached.BannerListAll);

            staticBanners = _cacheClient.Get<List<Entities.Banner>>(keyCached);

            if (staticBanners == null)
            {
                staticBanners = _bannerBoFE.GetAllActive();

                if (staticBanners != null)
                {
                    _cacheClient.Add(keyCached, staticBanners, _longExpiredInMinute);
                }
            }
            //}

            return staticBanners;
        }

        public List<Entities.Banner> GetByPageAndPosition(DVG.WIS.Core.Enums.BannerPageEnum pageId, DVG.WIS.Core.Enums.BannerPositionEnum position)
        {
            var lstBanner = this.GetAllActive();

            if (lstBanner != null)
            {
                lstBanner = lstBanner.FindAll(x => x.PageId == (int)pageId && x.Position == (int)position);
            }
            return lstBanner;
        }


        public List<Entities.Banner> GetByPageAndPosition(string currentUrl, int position, int blockId = -1)
        {
            var pageId = 0;
            foreach (var item in StaticVariable.DicBannerPageUrl)
            {
                if (item.Value == currentUrl)
                {
                    pageId = item.Key;
                }
            }

            var lstBanner = this.GetAllActive();

            if (lstBanner != null)
            {
                lstBanner = lstBanner.FindAll(x => x.PageId == pageId && x.Position == position);
            }
            return lstBanner;
        }
    }
}
