using System.Collections.Generic;

namespace DVG.WIS.DAL.AuthGroupCategoryMapping
{
    public interface IAuthGroupCategoryMappingDal
    {
        IEnumerable<Entities.AuthGroupCategoryMapping> GetByGrouId(int id);
        bool Insert(Entities.AuthGroupCategoryMapping obj);
        int DeleteByGroupId(int id);
    }
}
