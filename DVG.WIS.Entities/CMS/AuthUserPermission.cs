using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Entities
{
    [Serializable]
    public class AuthUserPermission
    {
        public AuthUserPermission()
        {
            LstPermissionAction = new List<AuthAction>();
            LstPermissionCategory = new List<Category>();
            LstPermissionNewsStatus = new List<int>();
        }
        public List<AuthAction> LstPermissionAction { get; set; }
        public List<Category> LstPermissionCategory { get; set; }
        public List<int> LstPermissionNewsStatus { get; set; }
    }
}
