using DVG.WIS.DAL.Infrastructure;
using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Repositories
{

    public interface IAuthGroupNewsStatusMappingRepository : IRepository<Entities.AuthGroupNewsStatusMapping>
    {
    }

    public class AuthGroupNewsStatusMappingRepository : RepositoryBase<Entities.AuthGroupNewsStatusMapping>, IAuthGroupNewsStatusMappingRepository
    {
        public AuthGroupNewsStatusMappingRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
