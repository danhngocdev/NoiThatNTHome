using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DVG.WIS.Business.Authenticator;
using DVG.WIS.Business.Orders;
using DVG.WIS.Business.Products;
using DVG.WIS.Core;
using DVG.WIS.Core.Constants;
using DVG.WIS.Core.Enums;
using DVG.WIS.Entities;
using DVG.WIS.Entities.Conditions;
using DVG.WIS.PublicModel.CMS;
using DVG.WIS.Utilities;

namespace DVG.CMS.Controllers
{
    public class OrderController : Controller
    {
        private IOrderBo _orderBo;
        private IProductBo _productBo;

        public OrderController(IOrderBo OrderBo, IProductBo productBo)
        {
            this._orderBo = OrderBo;
            this._productBo = productBo;
        }


        [IsValidUrlRequest(KeyName = "OrderController.Index", Description = "Order - Danh sách")]
        public ActionResult Index()
        {

            return View();

        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "OrderController.Search", Description = "Order - Danh sách")]
        public ActionResult Search(OrderSearchModel searchModel)
        {
            ResponseData responseData = new ResponseData();
            int totalRows = 0;
            if (string.IsNullOrEmpty(searchModel.StartDateStr))
                searchModel.StartDate = null;
            else
                searchModel.StartDate = Utils.ConvertStringToDateTime(searchModel.StartDateStr, Const.NormalDateFormat);
            if (string.IsNullOrEmpty(searchModel.EndDateStr))
                searchModel.EndDate = null;
            else
                searchModel.EndDate = Utils.ConvertStringToDateTime(searchModel.EndDateStr, Const.NormalDateFormat);

            OrderCondition OrderCondition = new OrderCondition()
            {
                StartDate = searchModel.StartDate.HasValue ? searchModel.StartDate.Value.Ticks : 0,
                EndDate = searchModel.EndDate.HasValue ? searchModel.EndDate.Value.Ticks : 0,
                PageIndex = searchModel.PageIndex,
                PageSize = searchModel.PageSize
            };
            var lstRet = _orderBo.GetList(OrderCondition, out totalRows);
            if (null != lstRet)
            {
                searchModel.EditItem = new OrderModel();
                searchModel.ListData = lstRet.Select(item => new OrderModel(item)).ToList();
                searchModel.ListStatus = EnumHelper.Instance.ConvertEnumToList<OrderStatusEnum>();
                searchModel.ListPaymentStatus = EnumHelper.Instance.ConvertEnumToList<PaymentStatusEnum>();
                searchModel.ListPaymentType = EnumHelper.Instance.ConvertEnumToList<PaymentTypeEnum>();

                responseData.Data = searchModel;
                responseData.TotalRow = totalRows;
                responseData.Success = true;
            }
            return Json(responseData);
        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "OrderController.SearchOrderDetail", Description = "OrderDetail - Danh sách")]
        public ActionResult GetOrderDetailByOrderID(int orderId)
        {
            ResponseData responseData = new ResponseData();
            var lstRet = _orderBo.GetListOrderDetail(orderId);
            if (null != lstRet)
            {
                var lstOrder = new List<OrderDetailModel>();
                lstOrder = lstRet.Select(item => new OrderDetailModel(item)).ToList();
                List<int> listProductId = lstRet.Select(x => x.ProductId).ToList();
                var listProduct = _productBo.GetListProducByListProductId(listProductId);
                if (listProduct != null && listProduct.Any())
                {
                    lstOrder.ForEach(item => item.Product = listProduct.Where(x => x.Id == item.ProductId).FirstOrDefault());
                }
                responseData.Data = lstOrder;
                responseData.Success = true;
            }
            return Json(responseData);
        }



        [HttpPost]
        [IsValidUrlRequest(KeyName = "OrderController.Search", Description = "Order - Cập nhật")]
        public ActionResult Update(OrderModel model)
        {
            ResponseData responseData = new ResponseData();

            if (null != model)
            {
                Order banner = model.Id > 0 ? _orderBo.GetOrderById(model.Id) : new Order();
                banner.AdminNote = model.AdminNote;
                banner.OrderStatus = model.OrderStatus;
                banner.PaymentStatus = model.PaymentStatus;
                banner.PaymentType = model.PaymentType;
                //Update banner
                ErrorCodes result = _orderBo.Update(banner);
                responseData.Success = result == ErrorCodes.Success;
                responseData.Message = StringUtils.GetEnumDescription(result);
            }
            return Json(responseData);
        }
    }
}