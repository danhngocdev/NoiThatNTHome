using DVG.WIS.Caching.DTO.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Caching.DTO
{
    public abstract class CachingContextBase : DbContext
    {
        public DbSet<KeyCacheModel> KeyCacheModels { get; set; }

        public string _nameOrConnectionString { get; set; }

        public CachingContextBase(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            this._nameOrConnectionString = nameOrConnectionString;
            this.KeyCacheModels = base.Set<KeyCacheModel>();
        }
        public CachingContextBase(DbConnection existsConnection)
            : base(existsConnection, true)
        {
            this._nameOrConnectionString = existsConnection.ConnectionString;
            this.KeyCacheModels = base.Set<KeyCacheModel>();
        }

        public virtual int UpdateKeys(KeyCacheModel model)
        {
            if (this.KeyCacheModels != null && this.KeyCacheModels.Any(m => m.Key == model.Key))
            {
                this.KeyCacheModels.Attach(model);
                this.Entry(model).State = EntityState.Modified;
            }
            else
            {
                this.KeyCacheModels.Add(model);
            }

            return 0;
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
