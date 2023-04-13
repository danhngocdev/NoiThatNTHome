using DVG.WIS.Utilities;
using FluentData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.InfoContact
{
    public class InfoContactDal : ContextBase, IInfoContactDal
    {
        public InfoContactDal()
        {
            _dbPosition = DBPosition.Master;
        }
        public IEnumerable<Entities.InfoContact> GetListPaging(string keyword, int status, int pageIndex, int pageSize,out int totalRows)
        {
			IEnumerable<WIS.Entities.InfoContact> listCategories;
			try
			{
				using (var context = Context())
				{
					IStoredProcedureBuilder cmd = context.StoredProcedure("Admin_InfoContact_GetListPaging")
						.Parameter("PageIndex", pageIndex, DataTypes.Int32)
						.Parameter("PageSize", pageSize, DataTypes.Int32)
						.Parameter("Keyword", keyword, DataTypes.String)
						.Parameter("Status", status, DataTypes.Int32)
						.ParameterOut("TotalRows", DataTypes.Int32);
					listCategories = cmd.QueryMany<WIS.Entities.InfoContact>();
					if (listCategories.Count() > 0)
					{
						totalRows = cmd.ParameterValue<int>("TotalRows");
					}
					else
					{
						totalRows = 0;
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}
			return listCategories;
		}

        public int Update(Entities.InfoContact infoContact)
        {//Admin_InfoContact_Update
            int numberRecords;
            using (IDbContext context = Context())
            {
                numberRecords = context.StoredProcedure("Admin_InfoContact_Update")
                    .Parameter("Id", infoContact.Id, DataTypes.Int32)
                    .Parameter("Name", infoContact.Name.Trim(), DataTypes.String)
                    .Parameter("Content", infoContact.Content.Trim(), DataTypes.String)
                    .Parameter("Phone", infoContact.Phone.Trim(), DataTypes.String)
                    .Parameter("Status", infoContact.Status, DataTypes.Int32)
                    .Execute();
            }
            return numberRecords;
        }
    }
}
