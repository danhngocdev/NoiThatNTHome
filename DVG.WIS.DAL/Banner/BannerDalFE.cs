using DVG.WIS.Utilities;
using FluentData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Banner
{
	public class BannerDalFE : ContextBase, IBannerDalFE
	{
		public List<Entities.Banner> GetAllActive()
		{
			string storeName = "FE_Banner_GetAllActive";
			try
			{
				using (IDbContext context = Context())
				{
					var lst = context.StoredProcedure(storeName)
						.QueryMany<Entities.Banner>();
					return lst;
				}
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
			}
		}
	}
}
