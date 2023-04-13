using System.ComponentModel;

namespace DVG.WIS.Core.Enums
{
    public enum RoleEnum
    {
        [Description("Phóng viên - cộng tác viên")]
        Reporter = 1,
        [Description("Biên tập viên")]
        Editor = 2,
        [Description("Thư ký chuyên mục")]
        ClericalSecretary = 3,
        [Description("Thư ký tòa soạn")]
        OfficeSecretary = 4,
        [Description("Tổng Biên Tập")]
        MasterEditor = 5,
        [Description("ADMINISTRATOR")]
        Administrator = 6
    }

    public enum UserTypeEnum
    {
        [Description("Quản trị viên")]
        UserAdmin = 10,
        [Description("Tài khoản biên tập viên")]
        Editor = 11
    }
}
