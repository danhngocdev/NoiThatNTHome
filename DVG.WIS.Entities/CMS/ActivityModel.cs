using DVG.WIS.Core;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Entities
{
    public class ActivityModel : Activity
    {
        public string UserName { get; set; }
        public string ActionDateString
        {
            get { return Utils.ConvertTicksToStringFormat(this.ActionDate.Ticks, Const.CustomeDateFormat); }
        }

        public string ObjectPopulate
        {
            get { return Core.CoreUtils.GetEnumActionType(ActionType); }
        }
    }
}
