using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Entities
{
    [Table("Order")]
    public class Order
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
        public string CustomerNote { get; set; }
        public string AdminNote { get; set; }

        public double TotalMoney { get; set; }
        public int OrderStatus { get; set; }
        public int PaymentType { get; set; }
        public int PaymentStatus { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedDateSpan { get; set; }

    }
}
