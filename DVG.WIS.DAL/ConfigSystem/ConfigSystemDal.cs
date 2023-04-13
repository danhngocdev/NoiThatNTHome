using System;
using System.Collections.Generic;
using DVG.WIS.DAL.ConfigSystem;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using FluentData;

namespace DVG.WIS.DAL.ConfigSystem
{
    public class ConfigSystemDal : ContextBase, IConfigSystemDal
    {
        public ConfigSystemDal()
        {
            _dbPosition = DBPosition.Master;
        }

        public int Update(WIS.Entities.ConfigSystem configSystem)
        {
            int number = 0;
            using (var context = Context())
            {
                var transaction = context.UseTransaction(true);
                try
                {
                    number = transaction.StoredProcedure("Admin_ConfigSystem_Update")
                    .Parameter("Name", configSystem.Name)
                    .Parameter("Value", configSystem.Value)
                    .Parameter("Enabled", configSystem.Enabled, DataTypes.Boolean)
                    .Execute();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }

            }
            return number;
        }

        public WIS.Entities.ConfigSystem Get(string keyName)
        {
            using (var context = Context())
            {
                return context.StoredProcedure("Admin_ConfigSystem_GetByName").Parameter("Name", keyName).QuerySingle<WIS.Entities.ConfigSystem>();
            }
        }

        public List<WIS.Entities.ConfigSystem> GetListConfig(string keyword, int status, int pageIndex, int pageSize, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var context = Context())
                {
                    var cmd = context.StoredProcedure("Admin_ConfigSystem_GetList")
                        .Parameter("Keyword", keyword)
                        .Parameter("Status", status)
                        .Parameter("PageIndex", pageIndex)
                        .Parameter("PageSize", pageSize)
                        .ParameterOut("TotalRow", DataTypes.Int32);
                    totalRows = cmd.ParameterValue<int>("TotalRow");
                    return cmd.QueryMany<WIS.Entities.ConfigSystem>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<string> ControlsGetHasPermessionByUserName(string userName)
        {
            return new List<string>();
        }

        public void UpdateControlSystem(ControlSystem controlSystem)
        {
            using (var context = Context())
            {
                var transaction = context.UseTransaction(true);
                try
                {
                    transaction.StoredProcedure("ControlSystem_Update")
                        .Parameter("Id", controlSystem.Id, DataTypes.Int32)
                        .Parameter("KeyName", controlSystem.KeyName, DataTypes.String)
                        .Parameter("Description", controlSystem.Description, DataTypes.String)
                        .Parameter("Controller", controlSystem.Controller, DataTypes.String)
                        .Parameter("IsEnabled", controlSystem.IsEnabled, DataTypes.Boolean)
                        .Execute();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }

            }
        }

        public int Delete(string name)
        {
            int number = 0;
            using (var context = Context())
            {
                var transaction = context.UseTransaction(true);
                try
                {
                    number = transaction.StoredProcedure("Admin_ConfigSystem_Delete")
                    .Parameter("Name", name, DataTypes.String)
                    .Execute();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }

            }
            return number;
        }
    }
}
