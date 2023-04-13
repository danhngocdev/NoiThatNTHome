using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Recruitments
{
    public interface IRecruitmentDal
    {
        IEnumerable<Entities.Recruitment> GetList(string position, string cateName, int status, int pageIndex, int pageSize, out int totalRow);
        Entities.Recruitment GetById(int id);
        int Update(Entities.Recruitment banner);
        int Delete(int id);
        IEnumerable<Entities.Recruitment> GetListFE();
    }
}
