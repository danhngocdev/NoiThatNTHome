using DVG.WIS.DAL.Infrastructure;
using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Repositories
{

    public interface IAuthGroupActionMappingRepository : IRepository<Entities.AuthGroupActionMapping>
    {
    }

    public class AuthGroupActionMappingRepository : RepositoryBase<Entities.AuthGroupActionMapping>, IAuthGroupActionMappingRepository
    {
        public AuthGroupActionMappingRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
