using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.FM_Photo
{
    public interface IFM_PhotoDal
    {
        int Update(WIS.Entities.FM_Photo obj);
        WIS.Entities.FM_Photo GetByFileName(string fileName);
        IEnumerable<WIS.Entities.FM_PhotoOnList> GetList(string keyword, string userName, int pageIndex, int pageSize, DateTime? fromDate = null, DateTime? toDate = null);
    }
}
