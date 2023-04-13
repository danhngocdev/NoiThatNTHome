using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.ProductShowHome
{
   public interface IProductShowHomeBo
    {
        IEnumerable<Entities.ProductShowHome> GetList(string keyword, int pageIndex, int pageSize, int status, out int totalRows);


        Entities.ProductShowHome GetById(int id);


        ErrorCodes Update(WIS.Entities.ProductShowHome priceList);


        ErrorCodes Delete(int id);
        IEnumerable<Entities.ProductShowHome> GetAllProductShowHome();


    }
}
