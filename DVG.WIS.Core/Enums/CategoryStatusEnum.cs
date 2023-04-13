using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Core.Enums
{
    public static class CategoryState
    {
        public enum CategoryStatusEnum
        {
			[Description("Đang hoạt động")]
            Active = 1,
			[Description("Đã xóa")]
			Deleted = 2,
			[Description("Đã khóa")]
			Lock = 4
        }
    }
    
}
