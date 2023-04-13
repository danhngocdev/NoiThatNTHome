using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Core
{
    public class Notify
    {
        public static string AddSuccess = "Thêm mới thành công";

        public static string AddError = "Thêm mới thất bại, vui lòng thử lại sau";

        public static string UpdateSuccess = "Cập nhật thành công";

        public static string UpdateError = "Cập nhật thất bại, vui lòng thử lại sau";

        public const string DeleteSuccess = "Xóa dữ liệu thành công";

        public const string DeleteError = "Xóa dữ liệu thất bại, vui lòng thử lại sau";

        public const string AccountExist = "Email hoặc số điện thoại đã được sử dụng";

        public const string SystemError = "Có lỗi xảy ra trong quá trình xử lý";

        public const string ContactSucess = "Gửi liên hệ thành công";
        public const string PaymentSuccess = "Đặt hàng thành công. Chúng tôi sẽ liên hệ với bạn trong thời gian sớm nhất";

        public const string AddCartSucess = "Thêm giỏ hàng thành công";
        public const string UpdateCartSucess = "Cập nhật giỏ hàng thành công";
        public const string NoHaveProductOnCart = "Bạn không có sản phẩm nào trong giỏ hàng";


    }
}
