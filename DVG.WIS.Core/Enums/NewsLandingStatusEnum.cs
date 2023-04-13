using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Core.Enums
{
    public enum NewsLandingStatusEnum
    {
        [Description("Đang hoạt động")]
        Active = 1,
        [Description("Đang bị khóa")]
        DeActive = 0,
    }
}
