using System.Collections.Generic;

namespace DVG.WIS.DAL.AuthGroupUserMapping
{
    public interface IAuthGroupUserMappingDal
    {
        IEnumerable<Entities.AuthGroupUserMapping> GetByUserId(int id);
        bool Insert(Entities.AuthGroupUserMapping obj);
        int DeleteByUserId(int id);
    }
}
