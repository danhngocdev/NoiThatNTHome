using DVG.WIS.DAL.Infrastructure;
using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Repositories
{
    public interface ICategoryRepository : IRepository<Entities.Category>
    {

    }
    public class CategoryRepository : RepositoryBase<Entities.Category>, ICategoryRepository
    {
        public CategoryRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
     }

}
