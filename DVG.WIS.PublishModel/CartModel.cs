using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.PublicModel
{
    public class CartModel
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { set; get; }
        public double Price { set; get; }
        public double Total { set; get; }
    }

    public class CartOrder
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
