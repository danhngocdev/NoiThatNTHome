using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Core.Enums
{
    public enum NewsLetterStatusEnum
    {
        [Description("Khởi tạo")]
        Init = 0,
        [Description("Đang xử lý")]
        Runing = 1,
        [Description("Đã xử lý xong")]
        Completed = 2,
    }
}
