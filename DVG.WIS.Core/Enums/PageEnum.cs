using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Core.Enums
{
    public enum PageEnum
    {
        [Description("Giới Thiệu")]
        About = 1,
        [Description("Quy Trình Thi Công")]
        Procedure = 2,
        [Description("Liên Hệ")]
        ContactUs = 3
    }
}
