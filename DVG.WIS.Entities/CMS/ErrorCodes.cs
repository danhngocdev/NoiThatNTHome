using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Entities
{
    public enum ErrorCodes
    {
        #region common
        [Description("Lỗi nghiệp vụ")]
        BusinessError = 500,
        [Description("Lỗi chưa xác định")]
        UnknowError = 501,
        [Description("Yêu cầu không hợp lệ")]
        InvalidRequest = 502,
        [Description("Lỗi ngoại lệ (Exception)")]
        Exception = 503,
        [Description("Thành công")]
        Success = 0,
        #endregion

        #region UpdateAccount
        [Description("Không tìm thấy thông tin tài khoản")]
        UpdateAccountUserNotFound = 1000,
        [Description("Tên đăng nhập không hợp lệ")]
        UpdateAccountInvalidUsername = 1001,
        [Description("Mật khẩu không hợp lệ")]
        UpdateAccountInvalidPassword = 1002,
        [Description("Mật khẩu nhập lại không chính xác")]
        UpdateAccountInvalidRetypePassword = 1003,
        [Description("Địa chỉ Email không hợp lệ")]
        UpdateAccountInvalidEmail = 1004,
        [Description("Số điện thoại di động không hợp lệ")]
        UpdateAccountInvalidMobile = 1005,
        [Description("Tên đăng nhập đã tồn tại")]
        UpdateAccountUsernameExists = 1006,
        [Description("Địa chỉ email đã tồn tại")]
        UpdateAccountEmailExists = 1007,
        [Description("Mật khẩu cũ không chính xác")]
        UpdateAccountInvalidOldPassword = 1008,
        [Description("Không tìm thấy thông tin quyền")]
        UpdateAccountPermissionNotFound = 1009,
        [Description("Không tìm thấy thông tin chuyên mục")]
        UpdateAccountZoneNotFound = 1010,
        [Description("Tài khoản đang bị khóa")]
        UpdateAccountUserHasBeenLocked = 1011,
        [Description("Mật khẩu cũ và mật khẩu mới không giống nhau")]
        UpdateAccountOldAndNewPasswordNotMatch = 1012,
        [Description("Trạng thái không hợp lệ")]
        UpdateAccountInvalidStatus = 1013,
        [Description("Bạn không có quyền sửa thông tin account này")]
        UpdateAccountCantNotEditSystem = 1014,
        #endregion

        #region ValidAccount
        [Description("Không tìm thấy tài khoản")]
        ValidAccountInvalidUsername = 2001,
        [Description("Mật khẩu không hợp lệ")]
        ValidAccountInvalidPassword = 2002,
        [Description("Tài khoản đang bị khóa")]
        ValidAccountUserLocked = 2003,
        [Description("Mã sms không hợp lệ")]
        ValidAccountInvalidSmsCode = 2004,
        [Description("Không có quyền xử lý")]
        ValidAccountNotHavePermission = 2005,
        #endregion

        #region SwitchCurrentRole
        [Description("Tài khoản không hợp lệ")]
        SwitchCurrentRoleUserNotFound = 02006,
        [Description("Vai trò không hợp lệ")]
        SwitchCurrentRoleInvalidRole = 02007,
        [Description("Tài khoản không có vai trò cần chuyển")]
        SwitchCurrentRoleUserNotHaveRole = 02008,
        #endregion

        #region Permission
        [Description("Lỗi chưa đăng nhập")]
        NotLogin = 101,
        [Description("Không được quyền truy cập")]
        AccessDenined = 102,
        [Description("Bạn không được quyền tạo [Chủ đề]")]
        NotAllowPostTopic = 103,
        [Description("Bạn không được quyền tạo [Bài viết]")]
        NotAllowPostThread = 104,
        [Description("Bạn không được quyền gửi bình luận")]
        NotAllowPostComment = 105,
        [Description("Bạn bị giới hạn quyền cảm ơn do chưa đạt cấp độ cao hơn")]
        NotEnoughQuotaToThanks = 106,
        [Description("Bạn bị giới hạn quyền tạo chủ đề do chưa đạt cấp độ cao hơn")]
        NotEnoughQuotaToPostTopic = 107,
        #endregion

        #region UserLogin
        [Description("Tên đăng nhập không hợp lệ")]
        AccountLoginInvalidUserName = 400,
        [Description("Mật khẩu không hợp lệ")]
        AccountLoginInvalidPassword = 401,
        [Description("Tên đăng nhập hoặc mật khẩu không chính xác")]
        AccountLoginWrongUserNameOrPassword = 402,
        [Description("Tài khoản của bạn đã bị khóa")]
        AccountLoginUserBanned = 403,
        [Description("Tài khoản của bạn đã bị xóa")]
        AccountLoginUserRemoved = 404,
        [Description("Mật khẩu mới không khớp")]
        AccountPasswordNotMatch = 405,
        [Description("Tài khoản chưa được kích hoạt")]
        AccountUnActive = 406,
        [Description("Bạn nhập sai Google code")]
        AccountWrongGoogleAuthenticator = 407,
        #endregion

        #region News
        [Description("Tiêu đề không được để trống")]
        NewsTitleEmpty = 600,
        [Description("Mô tả không được để trống")]
        NewsSapoEmpty = 601,
        [Description("Nội dung không được để trống")]
        NewsContentEmpty = 602,
        [Description("Người đăng không được để trống")]
        NewsCreatedByEmpty = 603,
        [Description("Bài viết không tồn tại")]
        NewsNotFound = 604,
        [Description("Url không đúng định dạng")]
        NewsInValidUrl = 605,
        [Description("Tiêu đề có từ quá dài")]
        NewsInValidTitle = 606,
        [Description("Trạng thái hiện tại không cho phép bạn gửi lên")]
        NewsSendFail = 607,
        [Description("Gửi tin thành công")]
        NewsSendSuccess = 608,
        [Description("Trạng thái hiện tại không cho phép bạn nhận tin hoặc tin đã được nhận bởi người khác")]
        NewsReciveFail = 609,
        [Description("Nhận tin thành công")]
        NewsReciveSuccess = 610,
        [Description("Tin đang được set nổi bật, xin vui lòng gỡ tin khỏi các danh sách nổi bật trước khi hạ tin")]
        NewsHighlightCannotUnPublish = 611,
        #endregion

        #region Account

        [Description("Tài khoản người dùng không tồn tại")]
        AccountNotExists = 700,

        [Description("Tên đăng nhập không hợp lệ")]
        UserInfoInvalidUserName = 701,

        [Description("Email không hợp lệ")]
        UserInfoInvalidEmail = 702,

        [Description("Tên đăng nhập đã tồn tại")]
        UserNameExisted = 703,
        [Description("Không gửi được mail")]
        SendMailError = 701,

        #endregion

        #region FAQ
        [Description("Chủ đề không tồn tại")]
        FaqTopicNotExists = 1100,
        [Description("Tiêu đề không đươc để trống")]
        FaqTitleEmpty = 1101,
        [Description("Câu hỏi không được để trống")]
        FaqQuestionEmpty = 1102,
        [Description("Câu hỏi không tồn tại")]
        FaqQuestionNotExist = 1103,
        [Description("Bình luận không tồn tại")]
        FaqParrentNotExist = 1104,
        [Description("Câu trả lời không được để trống")]
        FaqAnswerEmpty = 1105,
        [Description("Tên người hỏi không được để trống")]
        FaqFullNameEmpty = 1106,
        #endregion

        #region Register
        [Description("Email của bạn đã được đăng ký rồi.")]
        ExistedEmail = 800,

        [Description("Email bạn đăng ký không đúng định dạng.")]
        InvalidSubcribeEmail = 807,

        [Description("Username đã tồn tại")]
        ExistedAccount = 806,

        [Description("Số điện thoại đã tồn tại")]
        ExistedMobile = 801,

        [Description("Mã xác nhận không đúng")]
        WrongCapcha = 803,

        [Description("Các thông tin cần điền đầy đủ")]
        WrongOrNullInput = 804,

        [Description("Mail kích hoạt tài khoản lỗi")]
        ErrorActiveMail = 805,
        #endregion

        #region Tags

        [Description("Bản ghi đã tồn tại")]
        TagRecordExisted = 901,

        #endregion

        #region CarInfo

        [Description("Vị trí hiện tại đã tồn tại")]
        BannerExits = 4000,
        [Description("Không tìm thấy thông tin xe")]
        CarBrandNotFound = 4001,

        #endregion

        #region Topic

        [Description("Tên chủ đề không hợp lệ")]
        TopicNameInvalid = 5000,

        #endregion

        #region CarModels

        [Description("Tên của model không hợp lệ")]
        CarModelInvalidName = 6000,
        [Description("Không tìm thấy model")]
        CarModelNotFound = 6001,

        #endregion

        #region CarModelDetails

        [Description("Tên của phiên bản không hợp lệ")]
        CarModelDetailInvalidName = 7000,
        [Description("Không tìm thấy phiên bản xe")]
        CarModelDetailNotFound = 7001,

        #endregion

        #region LandingPage

        [Description("Link landing page đã tồn tại")]
        LandingPageLinkExisted = 8000,
        [Description("Link landing page không được để trống")]
        LandingPageLinkEmpty = 8001,
        [Description("Chưa có bài viết liên quan")]
        LandingPageNotNewsRelation = 8002,

        #endregion

        #region News link detail

        [Description("Link đã tồn tại")]
        NewsLinkDetailExisted = 9000,
        [Description("Link không được để trống")]
        NewsLinkDetailEmpty = 9001,

        #endregion

        #region Menu heading

        [Description("Tên menu không được để trống")]
        MenuHeadingNameEmpty = 10000,

        #endregion
       
    }
}
