using DVG.WIS.DAL.Infrastructure;
using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Repositories
{

    public interface IAuthGroupUserMappingRepository : IRepository<Entities.AuthGroupUserMapping>
    {
    }

    public class AuthGroupUserMappingRepository : RepositoryBase<Entities.AuthGroupUserMapping>, IAuthGroupUserMappingRepository
    {
        public AuthGroupUserMappingRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
