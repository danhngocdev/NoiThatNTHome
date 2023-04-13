using DVG.WIS.Business.Customers;
using DVG.WIS.Business.Products;
using DVG.WIS.Core;
using DVG.WIS.Core.Enums;
using DVG.WIS.Entities;
using DVG.WIS.Local;
using DVG.WIS.PublicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DVG.Website.Controllers
{
    public class CartController : Controller
    {
        private Message objMsg = new Message();
        private IProductBo _productBo;

        public CartController(IProductBo productBo)
        {
            _productBo = productBo;
        }

        public ActionResult CartNumber()
        {
            int countCart = 0;
            List<CartModel> lstCart = GetCart();
            if (lstCart != null && lstCart.Any())
            {
                countCart = lstCart.Sum(x => x.Quantity);
            }
            return PartialView("_CartNumber", countCart);
        }

        [HttpPost]
        public ActionResult AddCart(CartOrder cartOrder)
        {
            if (ModelState.IsValid)
            {
                var product = _productBo.GetById(cartOrder.ProductId);
                if (product == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                List<CartModel> lstCart = GetCart();
                var cart = new CartModel();
                if (lstCart != null && lstCart.Any())
                    cart = lstCart.FirstOrDefault(x => x.ProductId == cartOrder.ProductId);

                if (cart == null || cart.ProductId == 0)
                {
                    cart = new CartModel();
                    cart.Quantity = cartOrder.Quantity;
                    cart.ProductId = product.Id;
                    cart.Product = product;
                    cart.Price = product.PricePromotion > 0 ? product.PricePromotion.Value : product.Price;
                    cart.Total = cart.Price * cartOrder.Quantity;
                    lstCart.Add(cart);
                    objMsg.Title = Notify.AddCartSucess;
                    objMsg.NextAction = (int)NextAction.MessageAddCart;
                    return Json(objMsg);
                }
                else
                {
                    cart.Quantity++;
                    cart.Total = cart.Quantity * cart.Price;
                    objMsg.Title = Notify.UpdateCartSucess;
                    objMsg.NextAction = (int)NextAction.MessageAddCart;
                    return Json(objMsg);
                }
            }
            else
            {
                objMsg.Error = true;
                objMsg.Title = string.Join("</br>", ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage));
            }
            return Json(objMsg);
        }

        public ActionResult Checkout()
        {
            List<CartModel> lstCart = GetCart();
            if (lstCart == null || !lstCart.Any())
                return RedirectToAction("Index", "Home");
            return View(lstCart);
        }

        [HttpPost]
        public ActionResult RemoveCart(int productId)
        {
            List<CartModel> lstCart = GetCart();
            if (lstCart != null && lstCart.Any())
            {
                var product = lstCart.Where(x => x.ProductId == productId).FirstOrDefault();
                if (product != null)
                {
                    lstCart.Remove(product);
                }
                else
                {
                    objMsg.Error = true;
                    objMsg.Title = Notify.NoHaveProductOnCart;
                }
            }
            else
            {
                objMsg.Error = true;
                objMsg.Title = Notify.NoHaveProductOnCart;
            }
            return Json(objMsg);
        }


        public ActionResult Payment()
        {
            OrderModel model = new OrderModel();
            return PartialView("_Payment", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PaymentPost(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                var listCart = GetCart();
                if (listCart != null && listCart.Any())
                {
                    Order customer = new Order()
                    {
                        Name = model.Name,
                        Address = model.Address,
                        Phone = model.Phone,
                        Email = model.Email,
                        CustomerNote = model.Note,
                        PaymentType = model.PaymentType,
                        TotalMoney = listCart.Sum(x => x.Total)
                    };

                    List<OrderDetail> lstOrderDetail = new List<OrderDetail>();
                    foreach (var item in listCart)
                    {
                        lstOrderDetail.Add(new OrderDetail
                        {
                            Price = item.Price,
                            Quantity = item.Quantity,
                            ProductId = item.ProductId
                        });
                    }
                    ErrorCodes result = _productBo.UpdateOrder(customer, lstOrderDetail);

                    if (result == ErrorCodes.Success)
                    {
                        objMsg.Error = false;
                        objMsg.Title = Notify.PaymentSuccess;
                        objMsg.NextAction = (int)NextAction.ReloadPage;
                        Session[Const.SessionCart] = null;

                    }
                    else
                    {
                        objMsg.Error = true;
                        objMsg.Title = Notify.SystemError;
                    }
                }
                else
                {
                    objMsg.Error = true;
                    objMsg.Title = Notify.SystemError;
                }
            }
            else
            {
                objMsg.Error = true;
                objMsg.Title = string.Join("</br>", ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage));
            }
            return Json(objMsg);
        }


        #region Private Method

        private List<CartModel> GetCart()
        {
            List<CartModel> lstCart = Session[Const.SessionCart] as List<CartModel>;
            if (lstCart == null)
            {
                //Nếu giỏ hàng chưa tồn tại thì tiến ành tạo list giỏ hàng
                lstCart = new List<CartModel>();
                Session[Const.SessionCart] = lstCart;
            }
            return lstCart;
        }

        [HttpPost]
        public ActionResult UpdateCart(int productId, int type)
        {
            List<CartModel> lstCart = GetCart();
            CartModel cart = new CartModel();
            if (lstCart != null && lstCart.Any())
                cart = lstCart.FirstOrDefault(x => x.ProductId == productId);
            if (cart != null && cart.ProductId > 0)
            {
                if (type == 1)
                {
                    if (cart.Quantity == 1)
                    {
                        lstCart.Remove(cart);
                        cart = new CartModel();
                    }
                    else
                        cart.Quantity = cart.Quantity - 1;
                    cart.Total = cart.Price * cart.Quantity;
                }
                else if (type == 2)
                {
                    cart.Quantity = cart.Quantity + 1;
                    cart.Total = cart.Price * cart.Quantity;
                }
                else
                {
                    lstCart.Remove(cart);
                    cart = new CartModel();
                }
            }
            Session[Const.SessionCart] = lstCart;
            return Json(cart);
        }

        #endregion
    }
}