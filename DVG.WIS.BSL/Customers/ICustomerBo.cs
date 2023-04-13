using DVG.WIS.Entities;
using DVG.WIS.Entities.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.Customers
{
    public interface ICustomerBo
    {
        IEnumerable<Entities.Customer> GetList(CustomerCondition customer, out int totalRows);
        Entities.Customer GetById(int id);
        ErrorCodes Update(Entities.Customer banner);
        ErrorCodes UpdateSubcribe(Entities.Subscribe banner);
        ErrorCodes Delete(int id);
        IEnumerable<Customer> ExportExcel(string startDate = null, string endDate = null);
    }
}
