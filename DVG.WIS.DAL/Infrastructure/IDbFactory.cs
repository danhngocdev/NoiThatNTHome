using System;

namespace DVG.WIS.DAL.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        MyDbContext Init();
    }
}