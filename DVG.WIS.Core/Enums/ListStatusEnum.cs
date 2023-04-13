using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Core.Enums
{
    public enum ListStatusEnum
    {
        [Description("Hiển Thị")]
        Active = 1,
        [Description("Ẩn")]
        NonActive = 0
    }
}
