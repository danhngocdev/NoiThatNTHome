using DVG.WIS.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Entities
{
    [Table("Gallery")]
    public class Gallery
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [Required]
        [MaxLength(500)]
        public string Url { set; get; }
        public int Status { get; set; }
        public int Priority { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
