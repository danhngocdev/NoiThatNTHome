using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentData;
using System;
using System.Collections.Generic;

namespace DVG.WIS.DAL.Slider
{
    public class SliderDal : ContextBase, ISliderDal
    {
        public int Delete(int id)
        {
            string storeName = "Admin_Slider_DeleteById";
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

        public IEnumerable<Entities.Slider> GetAllSlider()
        {
            throw new NotImplementedException();
        }

        public Entities.Slider GetById(int id)
        {
            string storeName = "Admin_Slider_GetById";
            try
            {
                using (IDbContext context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("Id", id, DataTypes.Int32)
                        .QuerySingle<Entities.Slider>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public IEnumerable<Entities.Slider> GetList(string keyword, int pageIndex, int pageSize, out int totalRows)
        {
            throw new NotImplementedException();
        }

        public int Update(Entities.Slider slider)
        {
            string storeName = "Admin_Slider_Update";
            try
            {
                using (IDbContext context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("Id", slider.Id, DataTypes.Int32)
                        .Parameter("Name", slider.Name, DataTypes.String)
                        .Parameter("Avatar", slider.Name, DataTypes.String)
                        .Parameter("Link", slider.Name, DataTypes.String)
                        .Parameter("SortOrder", slider.SortOrder, DataTypes.Int32)            
                        .Parameter("Status", slider.Status, DataTypes.Int32)
                        .Parameter("CreatedDate", slider.CreatedDate, DataTypes.DateTime)
                        .Parameter("ModifiedDate", slider.ModifiedDate, DataTypes.DateTime)
                        .QuerySingle<int>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public int UpdateStatus(Entities.Slider slider)
        {
            throw new NotImplementedException();
        }
    }
}
