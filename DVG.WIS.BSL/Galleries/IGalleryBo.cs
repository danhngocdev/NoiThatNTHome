using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.Galleries
{
    public interface IGalleryBo
    {
        IEnumerable<Entities.Gallery> GetList(int status, int pageIndex, int pageSize, out int totalRows);
        IEnumerable<Entities.Gallery> GetListFE(int status,int pageSize);
        Entities.Gallery GetById(int id);
        ErrorCodes Update(Entities.Gallery banner);
        ErrorCodes Delete(int id);
    }
}
