using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Entities.Abstract
{
    public abstract class Auditable : IAuditable
    {
        public int Status { set; get; }
        public DateTime CreatedDate { set; get; }

        [MaxLength(256)]
        public string CreatedBy { set; get; }

        public DateTime ModifiedDate { set; get; }

        [MaxLength(256)]
        public string ModifiedBy { set; get; }


    }
}
