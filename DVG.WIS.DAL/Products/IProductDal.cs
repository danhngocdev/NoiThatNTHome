using DVG.WIS.Entities;
using DVG.WIS.Entities.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Products
{
    public interface IProductDal
    {
        IEnumerable<Entities.Product> GetList(ProductSearch productSearch, out int totalRows);
        Entities.Product GetById(int id);
        int Update(Entities.Product banner, List<Entities.NewsImage> listProductImage);
        int ChangeStatusProduct(int id, int statusProduct, string changeBy);
        IEnumerable<WIS.Entities.NewsImage> GetListImageByProductId(int ProductId);

        IEnumerable<Product> GetListProductNewest(int languageId,int limit);
        IEnumerable<Product> GetListProductHot(int limit);
        IEnumerable<Product> GetListProducByCateId(int cateId, int pageIndex, int pageSize, out int totalRows);
        IEnumerable<Product> GetListProducByKeyword(string keyword, int pageIndex, int pageSize, out int totalRows);

        int UpdateOrder(Order order, List<OrderDetail> orderDetails);
        IEnumerable<Product> GetListProducByListProductId(List<int> lstID);

        IEnumerable<Product> GetListProductSiteMap();

    }
}
