using DVG.WIS.Entities;
using DVG.WIS.Entities.Conditions;
using DVG.WIS.Utilities;
using FluentData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Orders
{
    public class OrderDal : ContextBase, IOrderDal
    {
        public IEnumerable<Order> GetList(OrderCondition customer, out int totalRows)
        {
            string storeName = "Admin_Order_GetList";
            IEnumerable<Entities.Order> lstRet;
            try
            {
                using (IDbContext context = Context())
                {
                    IStoredProcedureBuilder cmd = context.StoredProcedure(storeName)
                        .Parameter("StartDate", customer.StartDate, DataTypes.Int64)
                        .Parameter("EndDate", customer.EndDate, DataTypes.Int64)
                        .Parameter("PageIndex", customer.PageIndex, DataTypes.Int32)
                        .Parameter("PageSize", customer.PageSize, DataTypes.Int32)
                        .ParameterOut("TotalRows", DataTypes.Int32);
                    lstRet = cmd.QueryMany<Entities.Order>();
                    totalRows = cmd.ParameterValue<int>("TotalRows");
                    return lstRet;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public IEnumerable<OrderDetail> GetListOrderDetail(int orderId)
        {
            string storeName = "Admin_OrderDetail_GetByOrderId";
            IEnumerable<Entities.OrderDetail> lstRet;
            try
            {
                using (IDbContext context = Context())
                {
                    IStoredProcedureBuilder cmd = context.StoredProcedure(storeName)
                        .Parameter("OrderId", orderId, DataTypes.Int32);
                    lstRet = cmd.QueryMany<Entities.OrderDetail>();
                    return lstRet;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public Order GetOrderById(int orderId)
        {
            string storeName = "Admin_Order_GetById";
            try
            {
                using (IDbContext context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("OrderId", orderId, DataTypes.Int32)
                        .QuerySingle<Entities.Order>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public int Update(Order order)
        {
            string storeName = "Admin_Order_Update";
            try
            {
                using (IDbContext context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("Id", order.Id, DataTypes.Int32)
                        .Parameter("PaymentStatus", order.PaymentStatus, DataTypes.Int32)
                        .Parameter("OrderStatus", order.OrderStatus, DataTypes.Int32)
                        .Parameter("AdminNote", order.AdminNote, DataTypes.String)
                        .Parameter("ModifiedDate", DateTime.Now, DataTypes.DateTime)
                        .QuerySingle<int>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }
    }
}
