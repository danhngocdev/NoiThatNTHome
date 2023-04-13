using System;
using System.Collections.Generic;
using DVG.WIS.DAL.ConfigSystem;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;

namespace DVG.WIS.Business.ConfigSystem
{
    public class ConfigSystemBo : IConfigSystemBo
    {
        private IConfigSystemDal _configSystemDal;
        private bool _allowCached = true;

        public ConfigSystemBo(IConfigSystemDal configSystemDal)
        {
            _configSystemDal = configSystemDal;
            _allowCached = true;
        }

        public void Set(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _configSystemDal.Update(new WIS.Entities.ConfigSystem()
                {
                    Name = key,
                    Value = value
                });
            }
        }

        public WIS.Entities.ConfigSystem Get(string key, string defaultValue = "")
        {
            try
            {
                WIS.Entities.ConfigSystem configSystem = _configSystemDal.Get(key);

                if (!string.IsNullOrEmpty(defaultValue))
                {
                    if (configSystem == null || string.IsNullOrEmpty(configSystem.Name))
                    {
                        configSystem = new WIS.Entities.ConfigSystem()
                        {
                            Name = key,
                            Value = defaultValue
                        };

                        _configSystemDal.Update(configSystem);
                    }
                }
                return configSystem;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return new WIS.Entities.ConfigSystem();
            }
        }

        public List<string> ControlsGetHasPermessionByUserName(string userName)
        {
            return _configSystemDal.ControlsGetHasPermessionByUserName(userName);
        }

        public List<WIS.Entities.ConfigSystem> GetListConfig(string keyword, int status, int pageIndex, int pageSize, out int totalRows)
        {
            return _configSystemDal.GetListConfig(keyword, status, pageIndex, pageSize, out totalRows);
        }

        public ErrorCodes Update(WIS.Entities.ConfigSystem configSystem)
        {
            try
            {
                int number = _configSystemDal.Update(configSystem);

                if (_allowCached)
                {
                    //RemoveCacheByKey(configSystem.Name);
                }

                return number > 0 ? ErrorCodes.Success : ErrorCodes.BusinessError;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return ErrorCodes.Exception;
            }
            
        }

        public void ControlAdd(ControlSystem controlSystem)
        {
            _configSystemDal.UpdateControlSystem(controlSystem);
        }

        public ErrorCodes Delete(string name)
        {
            try
            {
                int number = _configSystemDal.Delete(name);
                return number > 0 ? ErrorCodes.Success : ErrorCodes.BusinessError;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return ErrorCodes.Exception;
            }
        }
    }
}
