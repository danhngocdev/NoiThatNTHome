using DVG.WIS.DAL.Infrastructure;
using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Repositories
{

    public interface INewsRepository : IRepository<Entities.News>
    {
        IEnumerable<Entities.News> GetListByCategoryId(int cateId);
    }

    public class NewsRepository : RepositoryBase<Entities.News>, INewsRepository
    {
        public NewsRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<Entities.News> GetListByCategoryId(int cateId)
        {
            return this.DbContext.News.Where(x => x.CategoryId == cateId);
        }
    }
}
