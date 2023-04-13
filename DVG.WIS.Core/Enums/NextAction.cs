using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Core.Enums
{
    public enum NextAction
    {
        ReloadPage = 1,
        Redirect = 2,
        MessageRedirect = 3,
        Message = 4,
        MessageNothing = 5,
        MessageAddCart = 6,
    }
}
