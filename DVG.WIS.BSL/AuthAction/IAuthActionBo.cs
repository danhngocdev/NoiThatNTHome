using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.AuthAction
{
	public interface IAuthActionBo
    {
        IEnumerable<Entities.AuthAction> GetAll();
        bool Insert(Entities.AuthAction obj);
    }
}
