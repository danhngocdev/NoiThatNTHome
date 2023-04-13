using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.ProductShowHome
{
    public interface IProductShowHomeDal
    {
        IEnumerable<Entities.ProductShowHome> GetList(string keyword, int pageIndex, int pageSize, int status, out int totalRows);
        Entities.ProductShowHome GetById(int id);
        int Update(Entities.ProductShowHome productShowHome);
        int Delete(int id);
        IEnumerable<Entities.ProductShowHome> GetListFE();
    }
}
