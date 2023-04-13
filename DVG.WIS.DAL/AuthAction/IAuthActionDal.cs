using System.Collections.Generic;

namespace DVG.WIS.DAL.AuthAction
{
    public interface IAuthActionDal
    {
        IEnumerable<Entities.AuthAction> GetAll();
        bool Insert(Entities.AuthAction obj);

    }
}
