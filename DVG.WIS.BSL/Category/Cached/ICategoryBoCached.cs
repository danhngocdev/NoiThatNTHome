using DVG.WIS.PublicModel;
using System.Collections.Generic;

namespace DVG.WIS.Business.Category.Cached
{
    public interface ICategoryBoCached
    {
        IEnumerable<WIS.Entities.Category> GetListAll();

        WIS.Entities.Category GetById(int id);

        Entities.Category GetByUrl(string url);        
    }
}
