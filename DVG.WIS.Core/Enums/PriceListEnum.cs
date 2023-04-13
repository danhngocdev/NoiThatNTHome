using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Core.Enums
{
   public enum PriceListStatusEnum
    {
        [Description("Hiển Thị")]
        Active = 1,
        [Description("Ẩn")]
        NonActive = 0
    }

    public enum PriceListUnitEnum
    {
        [Description("Mét Dài")]
         Meter = 1,
        [Description("Mét Vuông")]
        SquareMeters = 2,
        [Description("Chiếc")]
        One = 3

    }
}
