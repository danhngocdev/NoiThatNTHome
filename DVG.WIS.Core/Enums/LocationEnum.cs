using System.ComponentModel;

namespace DVG.WIS.Core.Enums
{
    public enum LocationStatusEnum
    {
        /// <summary>
        /// Hoạt động
        /// </summary>
        [Description("Hoạt động")]
        Enabled = 1,
        /// <summary>
        /// Vô hiệu hóa
        /// </summary>
        [Description("Vô hiệu hóa")]
        Disabled = 2
    }

    [Description("Vị trí của quận/huyện trong tỉnh/thành phố")]
    public enum DistrictPositionEnum
    {
        [Description("Nội thành")]
        Urban = 1,
        [Description("Ngoại thành")]
        Extramural = 2
    }

    [Description("Vị trí của quận/huyện trong tỉnh/thành phố dành cho TS/BĐS khác")]
    public enum OtherDistrictPositionEnum
    {
        [Description("Nội thành")]
        OtherUrban = 1,
        [Description("Ngoại thành")]
        OtherExtramural = 2
    }
}
