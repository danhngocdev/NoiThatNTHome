using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVG.WIS.Core.Enums;
using DVG.WIS.Entities;
using DVG.WIS.Entities.Conditions;
using DVG.WIS.Utilities;
using FluentData;

namespace DVG.WIS.DAL.Products
{
    public class ProductDal : ContextBase, IProductDal
    {
        public int ChangeStatusProduct(int id, int statusProduct, string changeBy)
        {
            string storeName = "Admin_Product_UpdateStatus";
            try
            {
                using (IDbContext context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("Id", id, DataTypes.Int32)
                        .Parameter("Status", statusProduct, DataTypes.Int32)
                        .Parameter("ModifiedBy", changeBy, DataTypes.String)
                        .Parameter("ModifiedDate", DateTime.Now, DataTypes.DateTime)
                        .QuerySingle<int>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public Entities.Product GetById(int id)
        {
            string storeName = "Admin_Product_GetById_20220819";
            try
            {
                using (IDbContext context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("Id", id, DataTypes.Int32)
                        .QuerySingle<Entities.Product>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public IEnumerable<Entities.Product> GetList(ProductSearch productSearch, out int totalRows)
        {
            string storeName = "Admin_Product_GetList";
            IEnumerable<Entities.Product> lstRet;
            try
            {
                using (IDbContext context = Context())
                {
                    IStoredProcedureBuilder cmd = context.StoredProcedure(storeName)
                        .Parameter("Status", productSearch.Status, DataTypes.Int32)
                        .Parameter("Keyword", productSearch.Keyword, DataTypes.String)
                        .Parameter("PageIndex", productSearch.PageIndex, DataTypes.Int32)
                        .Parameter("PageSize", productSearch.PageSize, DataTypes.Int32)
                        .ParameterOut("TotalRows", DataTypes.Int32);
                    lstRet = cmd.QueryMany<Entities.Product>();
                    totalRows = cmd.ParameterValue<int>("TotalRows");
                    return lstRet;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }
        public IEnumerable<Product> GetListProducByCateId(int cateId, int pageIndex, int pageSize, out int totalRows)
        {
            string storeName = "FE_Product_GetList";
            IEnumerable<Entities.Product> lstRet;
            try
            {
                using (IDbContext context = Context())
                {
                    IStoredProcedureBuilder cmd = context.StoredProcedure(storeName)
                        .Parameter("Status", ProductStatusEnum.Published.GetHashCode(), DataTypes.Int32)
                        .Parameter("CategoryId", cateId, DataTypes.Int32)
                        .Parameter("PageIndex", pageIndex, DataTypes.Int32)
                        .Parameter("PageSize", pageSize, DataTypes.Int32)
                        .ParameterOut("TotalRows", DataTypes.Int32);
                    lstRet = cmd.QueryMany<Entities.Product>();
                    totalRows = cmd.ParameterValue<int>("TotalRows");
                    return lstRet;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }
        public IEnumerable<Product> GetListProducByListProductId(List<int> ids)
        {
            try
            {
                using (IDbContext context = Context())
                {
                    string sql = string.Format("SELECT [Id],[Name],[Avatar],[Code],[Capacity] FROM [Products] WHERE [Id] IN({0}) ", string.Join(",", ids));
                    return context.Sql(sql)
                          .QueryMany<Entities.Product>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", "GetListProducByListProductId", ex.ToString()));
            }
        }
        public IEnumerable<Product> GetListProducByKeyword(string keyword, int pageIndex, int pageSize, out int totalRows)
        {
            string storeName = "FE_Product_GetList_Keyword";
            IEnumerable<Entities.Product> lstRet;
            try
            {
                using (IDbContext context = Context())
                {
                    IStoredProcedureBuilder cmd = context.StoredProcedure(storeName)
                        .Parameter("Status", ProductStatusEnum.Published.GetHashCode(), DataTypes.Int32)
                        .Parameter("Keyword", keyword, DataTypes.String)
                        .Parameter("PageIndex", pageIndex, DataTypes.Int32)
                        .Parameter("PageSize", pageSize, DataTypes.Int32)
                        .ParameterOut("TotalRows", DataTypes.Int32);
                    lstRet = cmd.QueryMany<Entities.Product>();
                    totalRows = cmd.ParameterValue<int>("TotalRows");
                    return lstRet;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public int Update(Entities.Product Product, List<Entities.NewsImage> listProductImage)
        {
            string storeName = "Admin_Product_Update_20220819";
            string storeInsertProductImage = "Admin_NewsImage_Insert";
            string storeDeleteProductImage = "Admin_NewsImage_DeleteByNewsId";
            int numberRecords = 0;
            var errors = string.Empty;

            using (IDbContext context = Context().UseTransaction(true))
            {
                try
                {
                    numberRecords = context.StoredProcedure(storeName)
                        .Parameter("Id", Product.Id, DataTypes.Int32)
                        .Parameter("Name", Product.Name, DataTypes.String)
                        .Parameter("Sapo", Product.Sapo, DataTypes.String)
                        .Parameter("Description", Product.Description, DataTypes.String)
                        .Parameter("Avatar", Product.Avatar, DataTypes.String)
                        .Parameter("Status", Product.Status, DataTypes.Int32)
                        .Parameter("CreatedBy", Product.CreatedBy, DataTypes.String)
                        .Parameter("ModifiedDate", DateTime.Now, DataTypes.DateTime)
                        .Parameter("ModifiedBy", Product.ModifiedBy, DataTypes.String)
                        .Parameter("SEOTitle", Product.SEOTitle, DataTypes.String)
                        .Parameter("SEODescription", Product.SEODescription, DataTypes.String)
                        .Parameter("SEOKeyword", Product.SEOKeyword, DataTypes.String)
                        .Parameter("TextSearch", Product.TextSearch, DataTypes.String)
                        .Parameter("Price", Product.Price, DataTypes.Double)
                        .Parameter("PricePromotion", Product.PricePromotion, DataTypes.Double)
                        .Parameter("Code", Product.Code, DataTypes.String)
                        .Parameter("MadeIn", Product.MadeIn, DataTypes.String)
                        .Parameter("Capacity", Product.Capacity, DataTypes.String)
                        .Parameter("IsOutStock", Product.IsOutStock, DataTypes.Int32)
                        .Parameter("IsHighLight", Product.IsHighLight, DataTypes.Int32)
                        .Parameter("CategoryId", Product.CategoryId, DataTypes.Int32)
                        .QuerySingle<int>();
                    if (numberRecords < 1)
                    {
                        errors = "Có lỗi ở sp " + storeName;
                    }
                    #region ProductImage
                    if (numberRecords > 0 && string.IsNullOrEmpty(errors))
                    {
                        //Xóa ds image cũ
                        context.StoredProcedure(storeDeleteProductImage)
                            .Parameter("NewsId", Product.Id, DataTypes.Int32)
                            .Execute();
                        //Insert ds image mới
                        if (listProductImage != null && listProductImage.Count > 0)
                        {
                            foreach (var image in listProductImage)
                            {
                                numberRecords = context.StoredProcedure(storeInsertProductImage)
                                    .Parameter("NewsId", Product.Id, DataTypes.Int32)
                                    .Parameter("ImageUrl", image.ImageUrl, DataTypes.String)
                                    .Parameter("Title", image.Title, DataTypes.String)
                                    .QuerySingle<int>();
                                if (numberRecords < 1)
                                {
                                    errors = "Có lỗi ở sp " + storeInsertProductImage;
                                    break;
                                }
                            }
                        }
                    }
                    #endregion

                    if (numberRecords > 0 && string.IsNullOrEmpty(errors))
                        context.Commit();
                    else
                        context.Rollback();
                }
                catch (Exception ex)
                {
                    context.Rollback();
                    throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
                }
            }
            return numberRecords;
        }


        public int UpdateOrder(Order order, List<OrderDetail> orderDetails)
        {
            string storeName = "FE_Order_Insert";
            string storeInsertProductImage = "FE_OrderDetail_Insert";
            int numberRecords = 0;
            var errors = string.Empty;

            using (IDbContext context = Context().UseTransaction(true))
            {
                try
                {
                    int orderId = context.StoredProcedure(storeName)
                         .Parameter("TotalMoney", order.TotalMoney, DataTypes.Double)
                         .Parameter("PaymentType", order.PaymentType, DataTypes.Int32)
                         .Parameter("Name", order.Name, DataTypes.String)
                         .Parameter("Phone", order.Phone, DataTypes.String)
                         .Parameter("Email", order.Email, DataTypes.String)
                         .Parameter("Address", order.Address, DataTypes.String)
                         .Parameter("CustomerNote", order.CustomerNote, DataTypes.String)
                         .Parameter("CreatedDateSpan", DateTime.Now.Ticks, DataTypes.Int64)
                         .QuerySingle<int>();
                    if (orderId < 1)
                    {
                        errors = "Có lỗi ở sp " + storeName;
                    }
                    #region ProductImage
                    if (orderId > 0 && string.IsNullOrEmpty(errors))
                    {
                        //Insert ds image mới

                        foreach (var item in orderDetails)
                        {
                            numberRecords = context.StoredProcedure(storeInsertProductImage)
                                .Parameter("OrderId", orderId, DataTypes.Int32)
                                .Parameter("ProductId", item.ProductId, DataTypes.Int32)
                                .Parameter("Price", item.Price, DataTypes.Double)
                                .Parameter("Quantity", item.Quantity, DataTypes.Int32)
                                .QuerySingle<int>();
                            if (numberRecords < 1)
                            {
                                errors = "Có lỗi ở sp " + storeInsertProductImage;
                                break;
                            }
                        }
                    }
                    #endregion

                    if (numberRecords > 0 && string.IsNullOrEmpty(errors))
                        context.Commit();
                    else
                        context.Rollback();
                }
                catch (Exception ex)
                {
                    context.Rollback();
                    throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
                }
            }
            return numberRecords;
        }
        public IEnumerable<WIS.Entities.NewsImage> GetListImageByProductId(int ProductId)
        {
            string storeName = "Admin_NewsImage_GetByNewsId";
            try
            {
                using (var context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("NewsId", ProductId, DataTypes.Int32)
                        .QueryMany<WIS.Entities.NewsImage>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public IEnumerable<Product> GetListProductNewest(int languageId, int limit)
        {
            string storeName = "FE_Product_GetListNewest_20220818";
            IEnumerable<Entities.Product> lstRet;
            try
            {
                using (IDbContext context = Context())
                {
                    IStoredProcedureBuilder cmd = context.StoredProcedure(storeName)
                        .Parameter("Top", limit, DataTypes.Int32)
                        .Parameter("Status", (int)ProductStatusEnum.Published, DataTypes.Int32);
                    lstRet = cmd.QueryMany<Entities.Product>();
                    return lstRet;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }
        public IEnumerable<Product> GetListProductHot(int limit)
        {
            string storeName = "FE_Product_GetListHot";
            IEnumerable<Entities.Product> lstRet;
            try
            {
                using (IDbContext context = Context())
                {
                    IStoredProcedureBuilder cmd = context.StoredProcedure(storeName)
                        .Parameter("Top", limit, DataTypes.Int32)
                        .Parameter("Status", (int)ProductStatusEnum.Published, DataTypes.Int32);
                    lstRet = cmd.QueryMany<Entities.Product>();
                    return lstRet;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public IEnumerable<Product> GetListProductSiteMap()
        {
            IEnumerable<Entities.Product> lstRet;
            try
            {
                using (IDbContext context = Context())
                {
                    string sql = "Select p.Id,p.Name,p.CreatedDate,p.Avatar from Products p where p.Status = 1 ";
                    lstRet =  context.Sql(sql)
                          .QueryMany<Entities.Product>();
                    return lstRet;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
