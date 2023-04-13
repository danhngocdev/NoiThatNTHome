using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Entities.Abstract
{
    public interface IAuditable
    {
        DateTime CreatedDate { set; get; }
        string CreatedBy { set; get; }
        DateTime ModifiedDate { set; get; }
        string ModifiedBy { set; get; }
        int Status { set; get; }
    }
}
