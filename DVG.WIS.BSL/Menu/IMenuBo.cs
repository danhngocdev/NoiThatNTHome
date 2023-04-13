using DVG.WIS.PublicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.Menu
{
    public interface IMenuBo
    {

        List<MenuTop> GetListMenuTop();
    }
}
