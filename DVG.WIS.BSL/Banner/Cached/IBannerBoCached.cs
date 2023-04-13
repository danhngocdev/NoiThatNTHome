using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.Banner.Cached
{
	public interface IBannerBoCached
	{
		List<Entities.Banner> GetAllActive();
        List<Entities.Banner> GetByPageAndPosition(DVG.WIS.Core.Enums.BannerPageEnum pageId, DVG.WIS.Core.Enums.BannerPositionEnum position);
        List<Entities.Banner> GetByPageAndPosition(string currentUrl, int position, int blockId = -1);
	}
}
