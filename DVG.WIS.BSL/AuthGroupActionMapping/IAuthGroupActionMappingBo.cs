using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.AuthGroupActionMapping
{
	public interface IAuthGroupActionMappingBo
    {
        IEnumerable<Entities.AuthGroupActionMapping> GetByGrouId(int id);
        bool Insert(Entities.AuthGroupActionMapping obj);
        ErrorCodes DeleteByGroupId(int id);
    }
}
