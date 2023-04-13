namespace DVG.WIS.DAL.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private MyDbContext dbContext;

        public MyDbContext Init()
        {
            return dbContext ?? (dbContext = new MyDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}