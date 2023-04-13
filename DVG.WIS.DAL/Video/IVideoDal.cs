using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Video
{
    public interface IVideoDal
    {
        IEnumerable<Entities.Video> GetList(string keyword, int pageIndex, int pageSize, int status, out int totalRows);
        Entities.Video GetById(int id);
        int Update(Entities.Video video);
        int Delete(int id);

        int UpdateStatus(Entities.Video video);

        IEnumerable<Entities.Video> GetListVideoTop(int top);
    }
}
