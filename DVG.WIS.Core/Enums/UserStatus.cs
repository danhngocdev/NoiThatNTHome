using System.ComponentModel;

namespace DVG.WIS.Core.Enums
{
    public enum UserStatus
    {
        Guest = 0,
        Registed = 1,
        Banned = 2,
        Removed = 3
    }

    public enum UserStatusAdmin
    {
        [Description("Không hoạt động")]
        Deactived = 0,
        [Description("Đang hoạt động")]
        Actived = 1,
        [Description("Đã xóa")]
        Deleted = 2
    }

    public enum SSOLogonStatus
    {
        [Description("Thành công")]
        Success = 100,
        [Description("Tài khoản SSO chưa được kết nối với tài khoản")]
        NotMapAccountToApp = 101,
        [Description("Có lỗi xảy ra khi đăng nhập")]
        BadData = 103,
    }
}
