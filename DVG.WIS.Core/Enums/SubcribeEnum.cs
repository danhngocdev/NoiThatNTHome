using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Core.Enums
{
	public enum SubscribeStatusEnum
	{
		[Description("Đang hoạt động")]
		Active = 1,
		[Description("Đã bị Ban")]
		Banned = 2
	}
}
