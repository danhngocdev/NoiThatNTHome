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
    [Table("Customers")]
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [Required]
        [MaxLength(256)]
        public string Name { set; get; }
        [Required]
        [MaxLength(500)]
        public string Address { get; set; }
        [Required]
        [MaxLength(50)]
        public string Phone { set; get; }
        [Required]
        [MaxLength(100)]
        public string Email { set; get; }
        [Required]
        public string Title { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedDateSpan { get; set; }
    }
}
