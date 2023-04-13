namespace DVG.WIS.Caching.DTO
{
    using System;
    using System.Collections.Generic;
    using Entities;
    using DVG.WIS.Caching.DTO.Interfaces;
    using System.Data.Entity;
    using System.Linq;
    using Utilities;
    using System.Data.Entity.Infrastructure;
    using Cached;

    public class CachingBo : ICachingBo
    {
        private string _connectionString = string.Empty;
        private CachingContextEnum.DBContextTypes _dbContextType = CachingContextEnum.DBContextTypes.SQL;

        public CachingBo()
        {
            this._connectionString = AppSettings.Instance.GetConnection("CachingConnection");
        }

        public CachingBo(string connectionString)
        {
            this._connectionString = connectionString;
            this._dbContextType = CachingContextEnum.DBContextTypes.SQL;
        }

        public CachingBo(string connectionString, CachingContextEnum.DBContextTypes dbContextType)
        {
            this._connectionString = connectionString;
            this._dbContextType = dbContextType;
        }

        public void Update(KeyCacheModel model)
        {
            if (string.IsNullOrEmpty(_connectionString)) return;

            if (string.IsNullOrEmpty(model.Key))
            {
                throw new Exception("Invalid key");
            }

            try
            {
                using (CachingContextBase db = SwitchCachedType(_dbContextType))
                {
                    db.UpdateKeys(model);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
        }

        public void Delete(string key)
        {
            if (string.IsNullOrEmpty(_connectionString)) return;

            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("Invalid key");
            }

            using (CachingContextBase db = SwitchCachedType(_dbContextType))
            {
                KeyCacheModel exists = (from item in db.KeyCacheModels
                                        where item.Key == key
                                        select item).FirstOrDefault();
                if (exists != null)
                {
                    db.KeyCacheModels.Remove(exists);
                    db.SaveChanges();
                }
            }
        }

        public KeyCacheModel GetByKey(string key)
        {
            if (string.IsNullOrEmpty(_connectionString)) return null;

            using (CachingContext db = new CachingContext(_connectionString))
            {
                KeyCacheModel exists = (from item in db.KeyCacheModels
                                        where item.Key == key
                                        select item).FirstOrDefault();
                return exists;
            }
        }

        public IEnumerable<KeyCacheModel> GetListByNameSpace(string ns)
        {
            if (string.IsNullOrEmpty(_connectionString)) return null;

            IEnumerable<KeyCacheModel> list;
            using (CachingContext db = new CachingContext(_connectionString))
            {
                list = (from item in db.KeyCacheModels
                        where item.Namespace == ns
                        select item);
            }
            return list;
        }

        public IEnumerable<KeyCacheModel> GetListByDate(DateTime fromDate, DateTime untilDate)
        {
            if (string.IsNullOrEmpty(_connectionString)) return null;

            IEnumerable<KeyCacheModel> list;
            using (CachingContext db = new CachingContext(_connectionString))
            {
                list = (from item in db.KeyCacheModels
                        where item.CreatedDate >= fromDate && item.CreatedDate <= untilDate
                        select item);
            }
            return list;
        }


        private CachingContextBase SwitchCachedType(CachingContextEnum.DBContextTypes dbContextType)
        {
            return CachingContextFactory.Instance.GetCachedType(dbContextType, _connectionString);
        }
    }
}
