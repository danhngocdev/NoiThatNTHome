using DVG.WIS.Caching.DTO.Entities;
using System.Data.Entity;
using System.Linq;

namespace DVG.WIS.Caching.DTO
{
    public partial class CachingContextSQL : CachingContextBase
    {
        public const string SchemaName = "dbo";
        public const string TableName = "KeyCache";

        public CachingContextSQL(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            //this.KeyCacheModels = base.Set<KeyCacheModel>();
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
