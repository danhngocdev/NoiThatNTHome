using DVG.WIS.DAL.Infrastructure;
using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Repositories
{
    public interface IBannerAdRepository : IRepository<BannerAd>
    {
    }

    public class BannerAdRepository : RepositoryBase<BannerAd>, IBannerAdRepository
    {
        public BannerAdRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
