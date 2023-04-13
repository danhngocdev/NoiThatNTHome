using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Core.Enums
{
    public enum OrderStatusEnum
    {
        [Description("Đơn hàng mới")]
        New = 0,
        [Description("Đơn hàng đang xử lý")]
        InProgess = 1,
        [Description("Hoàn thành")]
        Done = 2
    }
    public enum PaymentTypeEnum
    {
        [Description("Ship COD")]
        COD = 1,
        [Description("ATM")]
        ATM = 2
    }
    public enum PaymentStatusEnum
    {
        [Description("Chưa thanh toán")]
        NotYet = 0,
        [Description("Đã thanh toán")]
        Done = 1
    }
}
