using System.Collections.Generic;
using DVG.WIS.Entities;

namespace DVG.WIS.DAL.ConfigSystem
{
    public interface IConfigSystemDal
    {
        int Update(WIS.Entities.ConfigSystem configSystem);
        WIS.Entities.ConfigSystem Get(string keyName);
        List<WIS.Entities.ConfigSystem> GetListConfig(string keyword, int status, int pageIndex, int pageSize, out int totalRows);
        List<string> ControlsGetHasPermessionByUserName(string userName);
        void UpdateControlSystem(ControlSystem controlSystem);

        int Delete(string name);
    }
}
