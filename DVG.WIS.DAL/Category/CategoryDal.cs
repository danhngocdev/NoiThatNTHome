using System;
using System.Collections.Generic;
using DVG.WIS.Entities;
using FluentData;
using DVG.WIS.Utilities;
using System.Data;
using System.Linq;

namespace DVG.WIS.DAL.Category
{
	public class CategoryDal : ContextBase, ICategoryDal
	{
		#region Constructor

		public CategoryDal()
		{
			_dbPosition = DBPosition.Master;
		}

		#endregion

		public int Update(WIS.Entities.Category category)
		{
			int numberRecords;
			using (IDbContext context = Context())
			{
				numberRecords = context.StoredProcedure("Admin_Category_Update")
					.Parameter("Id", category.Id, DataTypes.Int32)
					.Parameter("ParentId", category.ParentId, DataTypes.Int32)
					.Parameter("Name", category.Name, DataTypes.String)
					.Parameter("Description", category.Description, DataTypes.String)
					.Parameter("SortOrder", category.SortOrder, DataTypes.Int32)
					.Parameter("Status", category.Status, DataTypes.Int32)
					.Parameter("Invisibled", category.Invisibled, DataTypes.Boolean)
					.Parameter("AllowComment", category.AllowComment, DataTypes.Boolean)
					.Parameter("Type", category.Type, DataTypes.Int32)
					.Parameter("ShortURL", category.ShortURL, DataTypes.String)
                    .Parameter("MetaTitle", category.MetaTitle, DataTypes.String)
                    .Parameter("MetaDescription", category.MetaDescription, DataTypes.String)
                    .Execute();
			}
			return numberRecords;
		}

		public bool Delete(int id, string deletedBy)
		{
			using (var context = Context())
			{
				int numberRecord = context.StoredProcedure("Admin_Category_Delete")
					.Parameter("Id", id)
					.Execute();
				if (numberRecord > 0)
				{
					return true;
				}
				return false;
			}
		}

		public WIS.Entities.Category GetById(int id)
		{
			using (var context = Context())
			{
				return context.StoredProcedure("Admin_Category_GetById")
					.Parameter("Id", id)
					.QuerySingle<WIS.Entities.Category>();
			}
		}

		public IEnumerable<WIS.Entities.Category> GetListByParent(int parentId = 0)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<WIS.Entities.Category> GetList(string keyword, int parentId)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<WIS.Entities.CategoryModel> GetListPaging(string keyword, int parentId, int newsType, int pageIndex, int pageSize, out int totalRows)
		{
			IEnumerable<WIS.Entities.CategoryModel> listCategories;
			try
			{
				using (var context = Context())
				{
					IStoredProcedureBuilder cmd = context.StoredProcedure("Admin_Category_GetListPaging")
						.Parameter("PageIndex", pageIndex, DataTypes.Int32)
						.Parameter("PageSize", pageSize, DataTypes.Int32)
						.Parameter("Keyword", keyword, DataTypes.String)
						.Parameter("ParentId", parentId, DataTypes.Int32)
						.Parameter("NewsType", newsType, DataTypes.Int32)
						.ParameterOut("TotalRow", DataTypes.Int32);
					listCategories = cmd.QueryMany<WIS.Entities.CategoryModel>();
					if (listCategories.Count() > 0)
					{
						totalRows = listCategories.First().TotalRow;
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

		public IEnumerable<CategoryModel> GetListByType(int type, int status)
		{
			string storeName = "Admin_Category_GetListByType";
			try
			{
				using (var context = Context())
				{
					return context.StoredProcedure(storeName)
						.Parameter("Type", type, DataTypes.Int32)
						.Parameter("Status", status, DataTypes.Int32)
						.QueryMany<CategoryModel>();
				}
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
			}
		}

        public IEnumerable<Entities.Category> GetByStatus(int status)
        {
            string storeName = "Admin_Category_GetByStatus";
            try
            {
                using (var context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("Status", status, DataTypes.Int32)
                        .QueryMany<Entities.Category>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public IEnumerable<CategoryModel> GetByStatusV2(int status)
		{
			string storeName = "Admin_Category_GetByStatus_V2";
			try
			{
				using (var context = Context())
				{
					return context.StoredProcedure(storeName)
						.Parameter("Status", status, DataTypes.Int32)
						.QueryMany<CategoryModel>();
				}
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
			}
		}

		public IEnumerable<WIS.Entities.NewsCategory> GetListNewsCategoryByNewsId(long newsId)
		{
			string storeName = "Admin_NewsCategory_GetByNewsId";
			try
			{
				using (var context = Context())
				{
					return context.StoredProcedure(storeName)
						.Parameter("NewsId", newsId, DataTypes.Int64)
						.QueryMany<WIS.Entities.NewsCategory>();
				}
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
			}
		}

        public IEnumerable<Entities.Category> GetListAll()
        {
            string storeName = "FE_Category_GetAll";

            try
            {
                using (IDbContext context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .QueryMany<WIS.Entities.Category>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex));
            }
        }
    }
}
