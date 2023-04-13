using System.ComponentModel;

namespace DVG.WIS.Core.Enums
{
    public enum NewsStatusEnum
    {
        [Description("Bài chờ xuất bản")]
        PendingApproved = 0,
        [Description("Bài đã xuất bản")]
        Published = 1,
        [Description("Bài bị gỡ")]
        UnPublished = 2,
        [Description("Bài bị xóa")]
        Deleted = 3
    }

    public enum NewsStatusAuthenEnum
    {
        [Description("Chờ xuất bản")]
        PendingApproved = 0,
        [Description("Xuất bản")]
        Published = 1,
        [Description("Gỡ bài")]
        UnPublished = 2,
        [Description("Xóa bài")]
        Deleted = 3,
    }

    public enum NewsDisplayPositionEnum
    {
        [Description("Bài bình thường")]
        Normal = 1,
        [Description("Bài nổi bật trang chủ")]
        Highlight = 2,
        [Description("Xem nhiều nhất")]
        MostView = 3
    }

    public enum NewsDisplayStyleEnum
    {
        /// <summary>
        /// Normal = 1: Bài bình thường
        /// </summary>
        [Description("Bài bình thường")]
        Normal = 1
    }

    public enum NewsPageTypeEnum
    {
        /// <summary>
        /// Trang chủ = 1
        /// </summary>
        [Description("Trang chủ")]
        HomePage = 1,
        /// <summary>
        /// Trang Giá xe máy = 2
        /// </summary>
        [Description("Trang danh sách")]
        ListPage = 2,
        /// <summary>
        /// Trang chi tiết = 3
        /// </summary>
        [Description("Trang chi tiết")]
        DetailPage = 3
    }

    public enum NewsTypeEnum
    {
        /// <summary>
        /// Tin tức = 1
        /// </summary>
        [Description("Tin tức")]
        News = 1,
    }

    public enum CategoryTypeEnum
    {
        /// <summary>
        /// Tin tức = 1
        /// </summary>
        [Description("Sản phẩm")]
        Product = 1,

        [Description("Tin Tức")]
        News = 2,
    }

    public enum NewsTypeStringEnum
    {
        /// <summary>
        /// Tin tức = 1
        /// </summary>
        [Description("tin-tuc")]
        News = 1,
    }

    public enum NewsStatusExternalEnum
    {
        [Description("Chờ duyệt")]
        Pending = 0,
        [Description("Đã duyệt")]
        Recived = 1
    }

    public enum NewsRelationStatusEnum
    {
        /// <summary>
        /// Đang hiển thị: Active = 1
        /// </summary>
        [Description("Đang hiển thị")]
        Active = 1,
        /// <summary>
        /// Đang được neo top: Sticky = 2
        /// </summary>
        [Description("Đang được neo top")]
        Sticky = 2,
        /// <summary>
        /// Đang bị block: Blocked = 3
        /// </summary>
        [Description("Đang bị block")]
        Blocked = 3
    }

    public enum NewsMostViewStatusEnum
    {
        /// <summary>
        /// Đang hiển thị: Active = 1
        /// </summary>
        [Description("Đang hiển thị")]
        Active = 1,
        /// <summary>
        /// Đang hiển thị: Active = 2
        /// </summary>
        [Description("Không hiển thị")]
        Deactive = 2,
        /// <summary>
        /// Đang bị block: Blocked = 3
        /// </summary>
        [Description("Đang bị block")]
        Blocked = 3
    }

    public enum NewsExternalType : int
    {
        All = 0, // tất cả tin
        Temp = 1, // tin lưu tạm
        Published = 2, // tin xuất bản
        Rollback = 3, //  tin bị trả
        TinxePublished = 4 // Lấy tin đã xuất bản sang Tinxe 
    }

    public enum DisplayStyleOnListEnum
    {
        [Description("Mặc định")]
        Normal = 0,
        [Description("Slide show")]
        Slide = 1,
        [Description("Ảnh lớn")]
        Cover = 2,
        [Description("Hiển thị cửa sổ khi click")]
        Alert = 3
    }

    public enum NewsCategoriesEnum
    {
        [Description("KIẾN THỨC LÀM ĐẸP")]
        Beautiful = 1,
        [Description("TIN DOANH NGHIỆP")]
        Company = 2,
        [Description("TIN TUYỂN DỤNG")]
        Career = 3,
        [Description("CÂU CHUYỆN KHÁCH HÀNG")]
        Customer = 4
    }
}
