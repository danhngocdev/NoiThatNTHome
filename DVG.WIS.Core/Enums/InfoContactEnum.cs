using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Core.Enums
{
    public enum  InfoContactEnum
    {
        [Description("Đã Tiếp Nhận")]
        Active = 1,
        [Description("Chưa Tiếp Nhận")]
        NonActive = 2
    }
}
