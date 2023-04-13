using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVG.WIS.Entities;
using DVG.WIS.DAL.Banner;
using DVG.WIS.Utilities;
using System.Web;

namespace DVG.WIS.Business.Banner
{
    public class BannerBo : IBannerBo
    {
        private IBannerDal _bannerDal;
        private const string KeyAllBanner = "KeyAllBanner";
        public BannerBo(IBannerDal bannerDal)
        {
            this._bannerDal = bannerDal;
        }

        public IEnumerable<Entities.Banner> GetList(string keyword,int platform, int position, int pageId, int status, int pageIndex, int pageSize, out int totalRows)
        {
            try
            {
                return _bannerDal.GetList(keyword, platform, position, pageId, status, pageIndex, pageSize, out totalRows);
            }
            catch (Exception ex)
            {
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, keyword, position, pageId, blockId, pageIndex, pageSize);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                totalRows = 0;
                return null;
            }
        }


        public IEnumerable<Entities.Banner> GetBannerByCondition(int pageId, int positionId, int platform)
        {
            List<Entities.Banner> lstAdsBannerModels = new List<Entities.Banner>();
            var lstAdsBanner = GetAllBanner();
            if (lstAdsBanner != null && lstAdsBanner.Any())
            {
                lstAdsBannerModels = lstAdsBanner.Where(x => x.PageId == pageId && x.Position == positionId && x.Platform == platform).ToList();
            }
            return lstAdsBannerModels;
        }

        public IEnumerable<Entities.Banner> GetAllBanner()
        {
            try
            {
                var tempAds = HttpContext.Current.Items[KeyAllBanner] as IEnumerable<Entities.Banner>;
                if (tempAds?.ToList().Count() > 0)
                {
                    return tempAds;
                }
                var lstBanner = _bannerDal.GetAllBanner();
                if (lstBanner != null && lstBanner.Any())
                {
                    HttpContext.Current.Items[KeyAllBanner] = lstBanner;
                    return lstBanner;
                }
                return null;
            }
            catch (Exception ex)
            {

                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return null;
            }
        }

        public Entities.Banner GetById(int id)
        {
            try
            {
                return _bannerDal.GetById(id);
            }
            catch (Exception ex)
            {
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, id);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return null;
        }

        public ErrorCodes Update(Entities.Banner banner)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                if (banner == null || banner.Id < 0)
                {
                    return ErrorCodes.BusinessError;
                }

                int result = _bannerDal.Update(banner);
                if (result < 1)
                {
                    return ErrorCodes.BusinessError;
                }
            }
            catch (Exception ex)
            {
                errorCodes = ErrorCodes.BusinessError;
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, banner);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return errorCodes;
        }

        public ErrorCodes Delete(int id)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                int result = _bannerDal.Delete(id);
                if (result < 1)
                {
                    return ErrorCodes.BusinessError;
                }
            }
            catch (Exception ex)
            {
                errorCodes = ErrorCodes.BusinessError;
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, id);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return errorCodes;
        }

        public ErrorCodes UpdateStatus(Entities.Banner banner)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                if (banner == null || banner.Id < 0)
                {
                    return ErrorCodes.BusinessError;
                }

                int result = _bannerDal.UpdateStatus(banner);
                if (result < 1)
                {
                    return ErrorCodes.BusinessError;
                }
            }
            catch (Exception ex)
            {
                errorCodes = ErrorCodes.BusinessError;
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, banner);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return errorCodes;
        }
    }
}
