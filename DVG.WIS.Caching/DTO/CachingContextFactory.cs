using System;
using System.Collections;
using System.Reflection;
using DVG.WIS.Caching.Cached.Implements;
using DVG.WIS.Caching.DTO.Entities;
using DVG.WIS.Caching.Cached;

namespace DVG.WIS.Caching.DTO
{
    public class CachingContextFactory
    {
        private readonly Hashtable _cachedMapping = new Hashtable();
        private static readonly object LockObject = new object();
        private static CachingContextFactory _instance;

        private CachingContextFactory()
        {
            Initialize();
        }

        public static CachingContextFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (LockObject)
                    {
                        if (_instance == null)
                            _instance = new CachingContextFactory();
                    }
                }

                return _instance;
            }
        }

        public void Initialize()
        {
            _cachedMapping[CachingContextEnum.DBContextTypes.SQL] = typeof(CachingContextSQL);
            _cachedMapping[CachingContextEnum.DBContextTypes.PostgreSQL] = typeof(CachingContextPostgreSQL);
            //_cachedMapping[CachingContextEnum.DBContextTypes.MySQL] = typeof(RedisCached);
            //_cachedMapping[CachingContextEnum.DBContextTypes.MySQL] = typeof(MemCached);
        }

        public CachingContextBase GetCachedType(CachingContextEnum.DBContextTypes cachedType)
        {
            CachingContextBase cached = null;

            if (_cachedMapping.ContainsKey(cachedType))
            {
                cached = Activator.CreateInstance((Type)_cachedMapping[cachedType]) as CachingContextBase;
            }

            return cached;
        }

        public CachingContextBase GetCachedType(CachingContextEnum.DBContextTypes cachedType, string connectionString)
        {
            CachingContextBase cached = null;

            if (_cachedMapping.ContainsKey(cachedType))
            {
                cached = Activator.CreateInstance((Type)_cachedMapping[cachedType], connectionString) as CachingContextBase;
            }

            return cached;
        }

    }
}
