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
    [Table("Persons")]
    public class Person : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [Required]
        [MaxLength(256)]
        public string Name { set; get; }

        [Required]
        [MaxLength(256)]
        public string Avatar { set; get; }

        [Required]
        public string Description { set; get; }

        [Required]
        [MaxLength(500)]
        public string Position { get; set; }

        [Required]
        public int Age { get; set; }
        [Required]
        public string Score { get; set; }
        public int Priority { get; set; }
        public int PageId { get; set; }
    }
}
