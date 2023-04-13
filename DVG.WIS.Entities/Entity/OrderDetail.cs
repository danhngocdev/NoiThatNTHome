using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Entities
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        [Key]
        public int OrderId { set; get; }
        [Key]
        public int ProductId { set; get; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}
