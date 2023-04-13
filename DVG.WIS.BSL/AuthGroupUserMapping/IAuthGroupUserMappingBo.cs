using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.AuthGroupUserMapping
{
	public interface IAuthGroupUserMappingBo
    {
        IEnumerable<Entities.AuthGroupUserMapping> GetByUserId(int id);
        bool Insert(Entities.AuthGroupUserMapping obj);
        ErrorCodes DeleteByGroupId(int id);
    }
}
