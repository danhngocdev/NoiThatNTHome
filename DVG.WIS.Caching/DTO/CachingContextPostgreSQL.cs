using DVG.WIS.Caching.DTO.Entities;
using DVG.WIS.Utilities.Databases;
using Npgsql;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace DVG.WIS.Caching.DTO
{
    //[DbConfigurationType(typeof(NpgsqlConfiguration))]
    public partial class CachingContextPostgreSQL : CachingContextBase
    {
        public const string SchemaName = "public";
        public const string TableName = "KeyCache";

        public CachingContextPostgreSQL(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            //this.KeyCacheModels = base.Set<KeyCacheModel>();
            //Helpful for debugging            
            //this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

            try
            {
                using (PostgresSQL dbContext = new PostgresSQL(_nameOrConnectionString, true))
                {
                    dbContext.CreateCommand(@"
                    CREATE TABLE " + TableName + @"(
	                    key varchar(200) NOT NULL,
	                    name_space varchar(200) NULL,
	                    created_date timestamp DEFAULT NOW() NOT NULL,
	                    created_datespan bigint DEFAULT 0 NOT NULL,
                      CONSTRAINT " + TableName + @"_pkey PRIMARY KEY(Key)
                    )
                    WITH (oids = false);").ExecuteNonQuery();
                }
            }
            catch
            {
                // Exception => Table existed
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<CachingContextPostgreSQL>(null);
            modelBuilder.HasDefaultSchema(SchemaName);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Properties().Configure(c =>
            {
                var name = c.ClrPropertyInfo.Name;
                var newName = name.ToLower();
                c.HasColumnName(newName);
            });

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<KeyCacheModel>().ToTable(TableName, SchemaName).HasKey(x => new { x.Key });
        }

        public override int UpdateKeys(KeyCacheModel model)
        {
            KeyCacheModel existed = null;
            using (PostgresSQL dbContext = new PostgresSQL(_nameOrConnectionString, true))
            {
                NpgsqlCommand cmd = dbContext.CreateCommand("SELECT * FROM " + TableName + @" WHERE key = '" + model.Key + @"'");
                NpgsqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    existed = new KeyCacheModel()
                    {
                        Key = reader["key"].ToString(),
                        Namespace = reader["namge_space"].ToString()
                    };
                }
                reader.Close();
            }

            model.CreatedDate = DateTime.Now;
            model.CreatedDateSpan = Utilities.Utils.DateTimeToUnixTime(model.CreatedDate);

            if (existed == null || string.IsNullOrEmpty(existed.Key))
            {
                string sqlCommand = @"
                INSERT INTO " + TableName + @"(key, name_space, created_datespan) 
                VALUES ('" + model.Key + "', '" + model.Namespace + "', " + model.CreatedDateSpan + ")";

                using (PostgresSQL dbContext = new PostgresSQL(_nameOrConnectionString, true))
                {
                    dbContext.CreateCommand(sqlCommand).ExecuteNonQuery();
                }
            }
            return 0;
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }

    //public class NpgsqlConfiguration
    //  : System.Data.Entity.DbConfiguration
    //{
    //    public NpgsqlConfiguration()
    //    {
    //        SetProviderServices("Npgsql", Npgsql.NpgsqlServices.Instance);
    //        SetProviderFactory("Npgsql", Npgsql.NpgsqlFactory.Instance);
    //        SetDefaultConnectionFactory(new Npgsql.NpgsqlConnectionFactory());
    //    }
    //}
}
