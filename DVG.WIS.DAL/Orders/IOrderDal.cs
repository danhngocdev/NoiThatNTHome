using DVG.WIS.Entities.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Orders
{
    public interface IOrderDal
    {
        IEnumerable<Entities.Order> GetList(OrderCondition customer, out int totalRows);
        IEnumerable<Entities.OrderDetail> GetListOrderDetail(int orderId);
        Entities.Order GetOrderById(int orderId);
        int Update(Entities.Order order);
    }
}
