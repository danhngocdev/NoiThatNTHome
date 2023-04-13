using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.Subscribe
{
	public interface ISubscribeBo
	{
		List<Entities.Subscribe> GetList(string email, int pageIndex, int pageSize, out int totalRows);
		ErrorCodes Delete(int id);
		ErrorCodes UpdateStatus(int id, string action);
	}
}
