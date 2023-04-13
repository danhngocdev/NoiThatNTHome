using System.Collections.Generic;

namespace DVG.WIS.DAL.AuthGroupNewsStatusMapping
{
    public interface IAuthGroupNewsStatusMappingDal
    {
        IEnumerable<Entities.AuthGroupNewsStatusMapping> GetByGrouId(int id);
        bool Insert(Entities.AuthGroupNewsStatusMapping obj);
        int DeleteByGroupId(int id);
    }
}
