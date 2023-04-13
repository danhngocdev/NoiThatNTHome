using DVG.WIS.Entities;
using DVG.WIS.PublicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.Persons
{
    public interface IPersonBo
    {
        IEnumerable<Person> GetList(string name, int? pageId, int? status, int pageIndex, int pageSize, out int totalRows);
        Entities.Person GetById(int id);
        ErrorCodes Update(Entities.Person banner);
        ErrorCodes Delete(int id);

        IEnumerable<Person> GetListFE(int pageId, int status, int limit);
    }
}
