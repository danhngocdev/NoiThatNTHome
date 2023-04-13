using DVG.WIS.DAL.Infrastructure;
using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Repositories
{
    public interface IPersonRepository:IRepository<Person>
    {

    }
    public class PersonRepository : RepositoryBase<Person>, IPersonRepository
    {
        public PersonRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
