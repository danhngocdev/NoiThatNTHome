using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Core.Enums
{
	public enum CarInfoSimilarStatusEnum
	{
		/// <summary>
		/// Đang hiển thị: Active = 1
		/// </summary>
		[Description("Đang hiển thị")]
		Active = 1,
		/// <summary>
		/// Đang được neo top: Sticky = 2
		/// </summary>
		[Description("Đang được neo top")]
		Sticky = 2,
		/// <summary>
		/// Đang bị block: Blocked = 3
		/// </summary>
		[Description("Đang bị block")]
		Blocked = 3
	}
}
