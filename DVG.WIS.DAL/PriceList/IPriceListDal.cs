using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.PriceList
{
    public interface IPriceListDal
    {
        IEnumerable<Entities.PriceList> GetList(string keyword, int pageIndex, int pageSize,int status, out int totalRows);
        Entities.PriceList GetById(int id);
        int Update(Entities.PriceList priceList);
        int Delete(int id);

        int UpdateStatus(Entities.PriceList priceList);

        IEnumerable<Entities.PriceList> GetListFe();
    }
}
