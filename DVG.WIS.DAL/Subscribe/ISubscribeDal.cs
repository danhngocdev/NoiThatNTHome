using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Subscribe
{
	public interface ISubscribeDal
	{
		List<Entities.Subscribe> GetList(string email, int pageIndex, int pageSize, out int totalRows);
		int Delete(int id);
		int UpdateStatus(int id, int status);
	}
}
