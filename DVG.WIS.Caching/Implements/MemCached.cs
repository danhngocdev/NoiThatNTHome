using DVG.WIS.Caching.DTO.Entities;
using DVG.WIS.Utilities;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using System;
using System.Net;
using System.Web;

namespace DVG.WIS.Caching.Cached.Implements
{
    public class MemCached : ICached, IDisposable
    {
        private static string _memcachedIp = AppSettings.Instance.GetString("MemcacedIP", "127.0.0.1");
        private static int _memcachedPort = AppSettings.Instance.GetInt32("MemcachedPort", 11211);
        private static string _memcachedAuthName = AppSettings.Instance.GetString("MemcachedAuthName");
        private static string _memcachedAuthPwd = AppSettings.Instance.GetString("MemcachedAuthPwd");
        private const string cacheInstance = "Memcache";

        private static MemcachedClientConfiguration config;
        private IMemcachedClient _memcache;

        public MemCached()
        {
            _memcache = Instance;
        }

        public MemCached(CachingConfigModel configuration)
        {
            if (configuration == null)
            {
                throw new Exception("Configuration not allow null");
            }
            if (string.IsNullOrEmpty(configuration.IpServer))
            {
                throw new Exception("Can not connection to server because server's IP undefined");
            }
            if (configuration.Port <= 0)
            {
                throw new Exception("Can not connection to server because server's Port undefined");
            }

            _memcachedIp = configuration.IpServer;
            _memcachedPort = configuration.Port;

            _memcachedAuthName = configuration.AuthName;
            _memcachedAuthPwd = configuration.AuthPassword;

            _memcache = Instance;
        }

        private IMemcachedClient Instance
        {
            get
            {
                IMemcachedClient _instance;


                config = new MemcachedClientConfiguration();

                config.Servers.Add(new IPEndPoint(IPAddress.Parse(_memcachedIp), _memcachedPort));
                config.Protocol = MemcachedProtocol.Binary;

                if (!string.IsNullOrEmpty(_memcachedAuthName))
                {
                    config.Authentication.Type = typeof(PlainTextAuthenticator);
                    config.Authentication.Parameters["userName"] = _memcachedAuthName;
                    config.Authentication.Parameters["password"] = _memcachedAuthPwd;
                }


                _instance = new MemcachedClient(config);


                return _instance;
            }
        }

        public bool Add<T>(string key, T item, int expireInMinute)
        {
            try
            {
                if (expireInMinute > 0)
                    return this._memcache.Store(StoreMode.Set, key, item, DateTime.Now.AddMinutes(expireInMinute));
                else
                    return this._memcache.Store(StoreMode.Set, key, item);
            }
            catch
            {
                return false;
            }
        }

        public bool Remove(string key)
        {
            try
            {
                return this._memcache.Remove(key);
            }
            catch
            {
                return false;
            }
        }

        public T Get<T>(string key)
        {
            try
            {
                return this._memcache.Get<T>(key);
            }
            catch
            {
                return default(T);
            }
        }

        public static CachedEnum.CachedTypes Key
        {
            get { return CachedEnum.CachedTypes.Memcached; }
        }

        public void Dispose()
        {
            if (_memcache != null)
            {
                _memcache.Dispose();
                _memcache = null;
            }
        }
    }
}
