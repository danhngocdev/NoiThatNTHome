using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Slider
{
  public interface ISliderDal
    {
        IEnumerable<Entities.Slider> GetList(string keyword, int pageIndex, int pageSize, out int totalRows);
        Entities.Slider GetById(int id);
        int Update(Entities.Slider slider);
        int Delete(int id);

        int UpdateStatus(Entities.Slider slider);

        IEnumerable<Entities.Slider> GetAllSlider();

    }
}
