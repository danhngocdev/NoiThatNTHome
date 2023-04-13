using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Banner
{
	public interface IBannerDal
	{
		IEnumerable<Entities.Banner> GetList(string keyword, int platform, int position, int pageId, int blockId, int pageIndex, int pageSize, out int totalRows);
		Entities.Banner GetById(int id);
		int Update(Entities.Banner banner);
		int Delete(int id);

        int UpdateStatus(Entities.Banner banner);

		IEnumerable<Entities.Banner> GetAllBanner();


	}
}
