using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.InfoContact
{
    public interface IInfoContactDal
    {
        int Update(WIS.Entities.InfoContact infoContact);
        IEnumerable<WIS.Entities.InfoContact> GetListPaging(string keyword,int status, int pageIndex, int pageSize,out int totalRows);

    }

}
