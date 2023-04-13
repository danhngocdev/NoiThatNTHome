using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Galleries
{
    public interface IGalleryDal
    {
        IEnumerable<Entities.Gallery> GetList(int status,  int pageIndex, int pageSize, out int totalRows);
        IEnumerable<Gallery> GetListFE(int status, int pageSize);
        Entities.Gallery GetById(int id);
        int Update(Entities.Gallery banner);
        int Delete(int id);
    }
}
