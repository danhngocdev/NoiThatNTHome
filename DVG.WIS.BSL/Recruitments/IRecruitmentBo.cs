using DVG.WIS.Entities;
using DVG.WIS.PublicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.Recruitments
{
    public interface IRecruitmentBo
    {
        IEnumerable<Entities.Recruitment> GetList(string position, string cateName,int status, int pageIndex, int pageSize, out int totalRows);
        Entities.Recruitment GetById(int id);
        ErrorCodes Update(Entities.Recruitment banner);
        ErrorCodes Delete(int id);
        IEnumerable<RecuitmentFEModel> GetListFE();
    }
}
