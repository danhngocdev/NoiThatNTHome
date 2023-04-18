using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.PublicModel
{
    public static class MyMenuDictionary
    {
        public static Dictionary<string, string> ListMenuFooterDictionary = new Dictionary<string, string>
            {
                {"Tủ Bếp","/danh-muc/phong-bep" },
                {"Phòng Khách","/danh-muc/phong-khach" },
                {"Phòng Ngủ","/danh-muc/phong-ngu" },
                {"Phụ Kiện","/danh-muc/phu-kien" },
                {"Video Thi Công","/video" },
                {"Công Trình Thi Công","/danh-muc/cong-trinh-thi-cong" }
            };
    }
}
