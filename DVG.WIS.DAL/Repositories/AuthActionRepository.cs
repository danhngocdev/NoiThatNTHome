using DVG.WIS.DAL.Infrastructure;
using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Repositories
{

    public interface IAuthActionRepository : IRepository<Entities.AuthAction>
    {
    }

    public class AuthActionRepository : RepositoryBase<Entities.AuthAction>, IAuthActionRepository
    {
        public AuthActionRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
