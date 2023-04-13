using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Core.Enums
{
    public enum CarBrandStatusEnum
    {
        [Description("Đang bị ẩn")]
        Disative = 0,
        [Description("Đang hiển thị")]
        Active = 1
    }
}
