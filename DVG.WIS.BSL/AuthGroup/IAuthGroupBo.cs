using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.AuthGroup
{
	public interface IAuthGroupBo
    {
        IEnumerable<Entities.AuthGroup> GetAll();
        int Insert(Entities.AuthGroup obj);
        Entities.AuthGroup GetById(int id);
        ErrorCodes Update(Entities.AuthGroup obj);
        ErrorCodes Delete(int id);
    }
}
