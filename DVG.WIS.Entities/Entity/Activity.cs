using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Entities
{
    [Table("Activity")]
    public class Activity
    {
        [Key]
        [Required]
        public long ActionTimeStamp { get; set; }
        public DateTime ActionDate { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ActionType { get; set; }
        public Nullable<long> ObjectId { get; set; }
        public string ActionText { get; set; }
    }
}
