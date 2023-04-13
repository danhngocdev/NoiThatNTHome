using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Persons
{
    public interface IPersonDal
    {
        IEnumerable<Entities.Person> GetList(string name, int? pageId, int? status, int pageIndex, int pageSize, out int totalRows);
        Entities.Person GetById(int id);
        int Update(Entities.Person banner);
        int Delete(int id);
        IEnumerable<Entities.Person> GetListFE();
        IEnumerable<Entities.Person> GetListFE(int pageId, int status, int limit);
    }
}
