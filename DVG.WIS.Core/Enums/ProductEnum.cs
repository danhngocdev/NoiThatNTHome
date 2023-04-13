using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Core.Enums
{
    public enum ProductStatusEnum
    {
        [Description("Sản phẩm chờ xuất bản")]
        PendingApproved = 0,
        [Description("Sản phẩm đã xuất bản")]
        Published = 1,
        [Description("Sản phẩm bị gỡ")]
        UnPublished = 2,
        [Description("Sản phẩm bị xóa")]
        Deleted = 3
    }
}
