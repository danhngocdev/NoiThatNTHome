using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Category
{
    public interface ICategoryDalFE
    {
        WIS.Entities.Category GetById(int cateId);

        IEnumerable<WIS.Entities.Category> GetAllByParent(int parentId, int status);

        IEnumerable<WIS.Entities.Category> GetListByParent(int parentId, int pageIndex, int pageSize);

        IEnumerable<WIS.Entities.Category> GetListAll();
        
    }
}
