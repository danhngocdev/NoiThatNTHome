using System.ComponentModel;

namespace DVG.WIS.Core.Enums
{
    public enum CustomerTypeEnum
    {
        [Description("Đăng ký khóa học tổng quan")]
        RegisterCourse = 0,
        [Description("Đăng ký chương trình học Ielts")]
        RegisterIelts = 1,
        [Description("Đăng ký chương trình Anh ngữ học thuật THCS")]
        RegisterAcademicStudy = 2,
        [Description("Đăng ký chương trình du học hè")]
        RegisterSummerCamp = 3,
        [Description("Đăng ký chương trình kĩ năng công dân")]
        RegisterCitizensSkill = 4,
        [Description("Đăng ký thông tin tuyển dụng")]
        RegisterRecruitment = 5

    }

    public enum CustomerSourceEnum
    {
        [Description("Trang chủ")]
        HomePage = 0,
        [Description("Trang luyện thi Ielts")]
        StudyForTest = 1,
        [Description("Trang Anh Ngữ học thuật THCS")]
        AcademicStudy = 2,
        [Description("Trang du học hè")]
        SummerCamp = 3,
        [Description("Trang kĩ năng công dân")]
        Skill = 4,
        [Description("Trang tuyển dụng")]
        Recruitment = 5,
        [Description("Trang giới thiệu")]
        AboutUs = 6,
    }

    public enum CustomerProgramEnum
    {
        [Description("Tất cả chương trình")]
        All = 0,
        [Description("Chương trình luyện thi Ielts")]
        Ielts = 1,
        [Description("Chương trình Anh ngữ THCS")]
        AcademicStudy = 2,
        [Description("Chương trình Kĩ năng công dân toàn cầu")]
        CitizensSkill = 3,
        [Description("Chương trình Du học hè")]
        SummerCamp = 4,
        [Description("Thông tin tuyển dụng")]
        Recruitment = 5,
    }
}