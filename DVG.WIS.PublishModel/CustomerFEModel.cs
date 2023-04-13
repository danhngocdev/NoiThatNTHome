using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.PublicModel
{
    public class CustomerFEModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên của bạn")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ email")]
        [RegularExpression(@"^[\w+][\w\.\-]+@[\w\-]+(\.\w{2,4})+$", ErrorMessage = "Email sai định dạng")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^(09|03|07|08|05)+([0-9]{8})$", ErrorMessage = "Số điện thoại sai định dạng")]
        public string Phone { get; set; }
        public string Title { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string Address { get; set; }
        public string Description { get; set; }
    }

    public class SubcribeModel
    {
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ email")]
        [RegularExpression(@"^[\w+][\w\.\-]+@[\w\-]+(\.\w{2,4})+$", ErrorMessage = "Email sai định dạng")]
        public string Email { get; set; }
     
    }
}
