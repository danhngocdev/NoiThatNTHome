using DVG.WIS.DAL.Orders;
using DVG.WIS.Entities;
using DVG.WIS.Entities.Conditions;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.Orders
{
    public class OrderBo:IOrderBo
    {
        public IOrderDal _orderDal;

        public OrderBo(IOrderDal orderDal)
        {
            _orderDal = orderDal;
        }

        public IEnumerable<Order> GetList(OrderCondition customer, out int totalRows)
        {
            try
            {
                return _orderDal.GetList(customer, out totalRows);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                totalRows = 0;
                return null;
            }
        }

        public IEnumerable<OrderDetail> GetListOrderDetail(int orderId)
        {
            try
            {
                return _orderDal.GetListOrderDetail(orderId);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return null;
            }
        }

        public Order GetOrderById(int orderId)
        {
            try
            {
                return _orderDal.GetOrderById(orderId);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return null;
        }

        public ErrorCodes Update(Order order)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                if (order == null || order.Id < 0)
                {
                    return ErrorCodes.BusinessError;
                }

                int result = _orderDal.Update(order);
                if (result < 1)
                {
                    return ErrorCodes.BusinessError;
                }
            }
            catch (Exception ex)
            {
                errorCodes = ErrorCodes.BusinessError;
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return errorCodes;
        }
    }
}
