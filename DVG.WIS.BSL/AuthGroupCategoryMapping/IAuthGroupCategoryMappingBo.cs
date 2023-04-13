using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.AuthGroupCategoryMapping
{
	public interface IAuthGroupCategoryMappingBo
    {
        IEnumerable<Entities.AuthGroupCategoryMapping> GetByGrouId(int id);
        bool Insert(Entities.AuthGroupCategoryMapping obj);
        ErrorCodes DeleteByGroupId(int id);
    }
}
