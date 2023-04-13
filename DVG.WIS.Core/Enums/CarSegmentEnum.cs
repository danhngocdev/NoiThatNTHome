using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Core.Enums
{
	public enum CarSegmentStatusEnum
	{
		[Description("Đang hiển thị")]
		Show = 1,
		[Description("Đã ẩn")]
		Hide = 2
	}
}
