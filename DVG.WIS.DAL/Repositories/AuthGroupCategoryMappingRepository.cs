using DVG.WIS.DAL.Infrastructure;
using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Repositories
{

    public interface IAuthGroupCategoryMappingRepository : IRepository<Entities.AuthGroupCategoryMapping>
    {
    }

    public class AuthGroupCategoryMappingRepository : RepositoryBase<Entities.AuthGroupCategoryMapping>, IAuthGroupCategoryMappingRepository
    {
        public AuthGroupCategoryMappingRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
