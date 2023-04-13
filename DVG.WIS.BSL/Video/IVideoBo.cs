using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.Video
{
    public interface IVideoBo
    {
        IEnumerable<Entities.Video> GetList(string keyword, int pageIndex, int pageSize, int status, out int totalRows);


        Entities.Video GetById(int id);


        ErrorCodes Update(WIS.Entities.Video video);


        ErrorCodes Delete(int id);


        IEnumerable<Entities.Video> GetListVideoTop(int top);
    }
}
