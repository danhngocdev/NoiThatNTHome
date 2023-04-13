using DVG.WIS.Utilities;
using FluentData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Subscribe
{
	public class SubscribeDal : ContextBase, ISubscribeDal
	{
		public List<Entities.Subscribe> GetList(string email, int pageIndex, int pageSize, out int totalRows)
		{
			string storeName = "Admin_Subscribe_GetList";
			List<Entities.Subscribe> lstRet;
			try
			{
				using (IDbContext context = Context())
				{
					IStoredProcedureBuilder cmd = context.StoredProcedure(storeName)
						.Parameter("Email", email, DataTypes.String)
						.Parameter("PageIndex", pageIndex, DataTypes.Int32)
						.Parameter("PageSize", pageSize, DataTypes.Int32)
						.ParameterOut("TotalRows", DataTypes.Int32);
					lstRet = cmd.QueryMany<Entities.Subscribe>();
					totalRows = cmd.ParameterValue<int>("TotalRows");
					return lstRet;
				}
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
			}
		}
	
		public int Delete(int id)
		{
			string storeName = "Admin_Subscribe_DeleteById";
			try
			{
				using (IDbContext context = Context())
				{
					return context.StoredProcedure(storeName)
						.Parameter("Id", id, DataTypes.Int32)
						.QuerySingle<int>();
				}
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
			}
		}
		public int UpdateStatus(int id, int status)
		{
			string storeName = "Admin_Subscribe_UpdateStatus";
			try
			{
				using (IDbContext context = Context())
				{
					return context.StoredProcedure(storeName)
						.Parameter("Id", id, DataTypes.Int32)
						.Parameter("Status", status, DataTypes.Int32)
						.QuerySingle<int>();
				}
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
			}
		}
	}
}
