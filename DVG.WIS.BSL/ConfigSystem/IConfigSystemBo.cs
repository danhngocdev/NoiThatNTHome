using System.Collections.Generic;
using DVG.WIS.Entities;

namespace DVG.WIS.Business.ConfigSystem
{
    public interface IConfigSystemBo
    {
        void Set(string key, string value);
        Entities.ConfigSystem Get(string key, string defaultValue = "");
        List<string> ControlsGetHasPermessionByUserName(string userName);
        List<WIS.Entities.ConfigSystem> GetListConfig(string keyword, int status, int pageIndex, int pageSize, out int totalRows);
        ErrorCodes Update(WIS.Entities.ConfigSystem configSystem);

        ErrorCodes Delete(string name);

        void ControlAdd(ControlSystem controlSystem);
    }
}
