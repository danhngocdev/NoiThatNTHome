using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.FM_photo
{
    public interface IFM_PhotoBo
    {
        IEnumerable<WIS.Entities.FM_PhotoOnList> GetList(string keyword, string userName, int pageIndex, int pageSize, DateTime? fromDate = null, DateTime? toDate = null);
        ErrorCodes Update(WIS.Entities.FM_Photo obj);
        WIS.Entities.FM_Photo GetByFileName(string fileName);
    }
}
