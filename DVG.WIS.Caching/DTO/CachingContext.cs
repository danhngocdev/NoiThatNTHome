using DVG.WIS.Caching.DTO.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVG.WIS.Caching.DTO.Entities;
using System.Data.Entity;
using DVG.WIS.Utilities;

namespace DVG.WIS.Caching.DTO
{
    public partial class CachingContext : CachingContextBase
    {
        public const string SchemaName = "dbo";
        public const string TableName = "KeyCache";

        public CachingContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            this.KeyCacheModels = base.Set<KeyCacheModel>();
        }

        public override int UpdateKeys(KeyCacheModel model)
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

            return SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<KeyCacheModel>().ToTable(TableName, SchemaName).HasKey(x => new { x.Key });
        }
    }
}
