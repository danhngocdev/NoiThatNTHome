using DVG.WIS.Entities;
using DVG.WIS.Entities.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.Orders
{
    public interface IOrderBo
    {
        IEnumerable<Entities.Order> GetList(OrderCondition customer, out int totalRows);
        IEnumerable<Entities.OrderDetail> GetListOrderDetail(int orderId);
        ErrorCodes Update(Entities.Order order);
        Order GetOrderById(int orderId);
    }
}
