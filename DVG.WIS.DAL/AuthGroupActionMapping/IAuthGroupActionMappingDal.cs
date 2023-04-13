using System.Collections.Generic;

namespace DVG.WIS.DAL.AuthGroupActionMapping
{
    public interface IAuthGroupActionMappingDal
    {
        IEnumerable<Entities.AuthGroupActionMapping> GetByGrouId(int id);
        bool Insert(Entities.AuthGroupActionMapping obj);
        int DeleteByGroupId(int id);
    }
}
