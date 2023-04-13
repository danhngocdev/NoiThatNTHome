using System.Collections.Generic;

namespace DVG.WIS.DAL.AuthGroup
{
    public interface IAuthGroupDal
    {
        IEnumerable<Entities.AuthGroup> GetAll();
        int Insert(Entities.AuthGroup obj);
        Entities.AuthGroup GetById(int id);
        int Update(Entities.AuthGroup obj);
        int Delete(int id);
    }
}
