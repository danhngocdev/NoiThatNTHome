using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Entities
{
    [Table("Banner")]
    public class Banner
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Embed { get; set; }
        public int Status { get; set; }
        public int Platform { get; set; }
        public int? Position { get; set; }
        public int? PageId { get; set; }
        public string TargetLink { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? UntilDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}
