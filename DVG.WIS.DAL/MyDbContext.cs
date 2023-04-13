using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL
{
    public class MyDbContext : DbContext
    {
        public MyDbContext() : base("MyDBConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<Entities.AuthAction> AuthAction { set; get; }
        public DbSet<Entities.AuthGroup> AuthGroup { set; get; }
        public DbSet<Entities.AuthGroupActionMapping> AuthGroupActionMapping { set; get; }
        public DbSet<Entities.AuthGroupCategoryMapping> AuthGroupCategoryMapping { set; get; }
        public DbSet<Entities.AuthGroupNewsStatusMapping> AuthGroupNewsStatusMapping { set; get; }
        public DbSet<Entities.AuthGroupUserMapping> AuthGroupUserMapping { set; get; }
        public DbSet<User> Users { set; get; }
        public DbSet<Entities.News> News { set; get; }
        public DbSet<Customer> Customers { set; get; }
        public DbSet<Person> Persons { set; get; }
        public DbSet<BannerAd> BannerAds { set; get; }
        public DbSet<Entities.Category> Categories { set; get; }
        public DbSet<UserRole> UserRole { set; get; }
        public DbSet<Activity> Activity { set; get; }
        public DbSet<Entities.ConfigSystem> ConfigSystem { set; get; }
        public DbSet<Permission> Permission { set; get; }
        public DbSet<ControlSystem> ControlSystem { set; get; }
        public static MyDbContext Create()
        {
            return new MyDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder builder)
        {

        }
    }
}
