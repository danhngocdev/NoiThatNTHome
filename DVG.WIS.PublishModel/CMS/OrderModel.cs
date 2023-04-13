using System;
using System.Collections.Generic;
using DVG.WIS.Core;
using DVG.WIS.Core.Constants;

namespace DVG.WIS.PublicModel.CMS
{
    public class OrderModel
    {
        public OrderModel() { }
        public OrderModel(Entities.Order order)
        {
            this.Id = order.Id;
            this.Name = order.Name;
            this.Email = order.Email;
            this.Address = order.Address;
            this.Phone = order.Phone;
            this.PaymentType = order.PaymentType;
            this.OrderStatus = order.OrderStatus;
            this.PaymentStatus = order.PaymentStatus;
            this.TotalMoney = order.TotalMoney;
            this.CustomerNote = order.CustomerNote;
            this.AdminNote = order.AdminNote;

            this.CreatedDate = order.CreatedDate;
            this.ModifiedDate = order.ModifiedDate;
        }
        public int Id { get; set; }
        public int PaymentType { get; set; }
        public int PaymentStatus { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public double TotalMoney { get; set; }
        public int OrderStatus { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CustomerNote { get; set; }
        public string AdminNote { get; set; }
        public string CreatedDateStr
        {
            get { return CreatedDate != null ? CreatedDate.ToString(Const.NormalDateFormat) : string.Empty; }
            set { }
        }
        public string ModifiedDateStr
        {
            get { return ModifiedDate != null ? ModifiedDate.ToString(Const.NormalDateFormat) : string.Empty; }
            set { }
        }
        public string TotalMoneyStr
        {
            get
            {
                return string.Format("{0:#,##0} VND", TotalMoney);
            }
        }
        public string ActiveCode { get; set; }
    }
    public class OrderSearchModel
    {
        public OrderSearchModel()
        {
            this.EditItem = new OrderModel();
        }
        public DateTime? StartDate { get; set; }
        public string StartDateStr { get; set; }
        public DateTime? EndDate { get; set; }
        public string EndDateStr { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<DVG.WIS.Utilities.EnumHelper.Enums> ListStatus { get; set; }
        public IEnumerable<DVG.WIS.Utilities.EnumHelper.Enums> ListPaymentStatus { get; set; }
        public IEnumerable<DVG.WIS.Utilities.EnumHelper.Enums> ListPaymentType { get; set; }
        public List<OrderModel> ListData { get; set; }
        public OrderModel EditItem { get; set; }
    }

    public class OrderDetailModel
    {
        public OrderDetailModel()
        {
        }

        public OrderDetailModel(Entities.OrderDetail order)
        {
            this.OrderId = order.OrderId;
            this.ProductId = order.ProductId;
            this.Quantity = order.Quantity;
            this.Price = order.Price;
            this.Product = new Entities.Product();

        }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public Entities.Product Product { get; set; }
        public string PriceStr
        {
            get
            {
                return string.Format("{0:#,##0} VND", Price);
            }
        }
    }
}