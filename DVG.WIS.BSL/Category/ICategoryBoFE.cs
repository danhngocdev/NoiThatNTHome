using DVG.WIS.PublicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.Category
{
    public interface ICategoryBoFE
    {
        IEnumerable<WIS.Entities.Category> GetList(int parentId);
        WIS.Entities.Category GetById(int id);
        IEnumerable<WIS.Entities.Category> GetListAll();


    }
}
