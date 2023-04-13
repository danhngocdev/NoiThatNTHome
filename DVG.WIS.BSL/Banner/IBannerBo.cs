using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.Banner
{
	public interface IBannerBo
	{
		IEnumerable<Entities.Banner> GetList(string keyword, int platform, int position, int pageId, int status, int pageIndex, int pageSize, out int totalRows);
		Entities.Banner GetById(int id);
		ErrorCodes Update(Entities.Banner banner);
		ErrorCodes Delete(int id);
		ErrorCodes UpdateStatus(Entities.Banner banner);

		IEnumerable<Entities.Banner> GetBannerByCondition(int pageId,int positionId,int platform);
	}
}
