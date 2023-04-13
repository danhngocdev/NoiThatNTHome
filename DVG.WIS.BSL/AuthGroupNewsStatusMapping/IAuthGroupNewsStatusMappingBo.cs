using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.AuthGroupNewsStatusMapping
{
	public interface IAuthGroupNewsStatusMappingBo
    {
        IEnumerable<Entities.AuthGroupNewsStatusMapping> GetByGrouId(int id);
        bool Insert(Entities.AuthGroupNewsStatusMapping obj);
        ErrorCodes DeleteByGroupId(int id);
    }
}
