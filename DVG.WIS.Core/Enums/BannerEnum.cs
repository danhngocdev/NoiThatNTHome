using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Core.Enums
{
    public enum BannerStatusEnum
    {
        [Description("Đang hiển thị")]
        Show = 1,
        [Description("Đã ẩn")]
        Hide = 2
    }

    public enum BannerPlatformEnum
    {
        [Description("Desktop")]
        Web = 1,
        [Description("Mobile")]
        Wap = 2
    }
    public enum BannerPositionEnum
    {
        [Description("Head Banner (Web:1920x500) (Wap:390x260)")]
        Main = 1,
        [Description("Banner Mid 1 (Web:370x230) (Wap:165x102)")]
        Mid1 = 2,
        [Description("Banner Mid 2  (Web:270x470) (Wap:165x102)")]
        Mid2 = 3,
        [Description("Banner Mid 3 (Web:1920x385) (Wap:165x102)")]
        Mid3 = 4,
        [Description("Banner Sticky (Web:380x185) (Wap:300x100)")]
        Sticky = 5,
    }
    public enum BannerPageEnum
    {
        [Description("Trang chủ")]
        HomePage = 1,
        [Description("Trang giới thiệu")]
        Introduce = 2,
        [Description("Trang giá trị cốt lõi")]
        CoreValues = 3,
        [Description("Trang tin tức")]
        News = 4,
        [Description("Trang sản phẩm")]
        Product = 5,
        [Description("Trang câu chuyện thành công")]
        SuccessStories = 6
    }
}
