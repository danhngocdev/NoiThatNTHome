using DVG.WIS.DAL.Infrastructure;
using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Repositories
{

    public interface IAuthGroupRepository : IRepository<Entities.AuthGroup>
    {
    }

    public class AuthGroupRepository : RepositoryBase<Entities.AuthGroup>, IAuthGroupRepository
    {
        public AuthGroupRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
