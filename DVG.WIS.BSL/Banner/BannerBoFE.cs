using DVG.WIS.DAL.Banner;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.Banner
{
	public class BannerBoFE : IBannerBoFE
	{
		private IBannerDalFE _bannerDalFE;
		public BannerBoFE(IBannerDalFE bannerDalFE)
		{
			this._bannerDalFE = bannerDalFE;
		}
		public List<Entities.Banner> GetAllActive()
		{
			try
			{
				return _bannerDalFE.GetAllActive();
			}
			catch (Exception ex)
			{
				Logger.WriteLog(Logger.LogType.Error, ex.ToString());
			}
			return null;
		}
	}
}
