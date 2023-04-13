using DVG.WIS.Entities;
using DVG.WIS.PublicModel.CMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.PriceList
{
   public interface IPriceListBo
    {
        IEnumerable<Entities.PriceList> GetList(string keyword, int pageIndex, int pageSize,int status, out int totalRows);


        Entities.PriceList GetById(int id);


        ErrorCodes Update(WIS.Entities.PriceList priceList);


        ErrorCodes Delete(int id);


        IEnumerable<PriceListModel> GetListAllFe();

    }
}
