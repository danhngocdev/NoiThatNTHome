using DVG.WIS.Entities;
using DVG.WIS.Entities.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Customers
{
    public interface ICustomerDal
    {
        IEnumerable<Entities.Customer> GetList(CustomerCondition customer, out int totalRows);
        Entities.Customer GetById(int id);
        int Update(Entities.Customer banner);
        int UpdateSubcribe(Entities.Subscribe banner);
        int Delete(int id);

        IEnumerable<Customer> ExportExcel(DateTime? startDate = null, DateTime? endDate = null);
    }
}
