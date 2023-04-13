using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.InfoContact
{
   public interface IInfoContactBo 
    {
        IEnumerable<Entities.InfoContact> GetList(string keyword, int pageIndex, int pageSize, int status, out int totalRows);
        ErrorCodes Update(WIS.Entities.InfoContact infoContact);
    }
}
