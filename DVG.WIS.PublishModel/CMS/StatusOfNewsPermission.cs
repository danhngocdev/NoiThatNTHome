using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.PublicModel.CMS
{
    [Serializable]
    public class StatusOfNewsPermission
    {
        public bool IsPendingApprove { get; set; }
        public bool IsPublish { get; set; }
        public bool IsUnPublish { get; set; }
        public bool IsDelete { get; set; }
    }
}
