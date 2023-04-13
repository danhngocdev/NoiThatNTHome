using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Banner
{
	public interface IBannerDalFE
	{
		List<Entities.Banner> GetAllActive();
	}
}
