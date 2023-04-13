using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.PublicModel
{
   public class ContactFEModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên")]

        public string Name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^(09|03|07|08|05|\+84)+([0-9]{8,9})$", ErrorMessage = "Số điện thoại sai định dạng")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập nội dung")]
        public string Content { get; set; }
        public int Status { get; set; }
       
    }



}
