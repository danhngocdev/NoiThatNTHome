using System;
using System.Web;
using System.Web.Caching;

namespace DVG.WIS.Caching.Cached.Implements
{
    public class IISCached : ICached, IDisposable
    {
        //private Cache _cache;
        public IISCached()
        {
            //_cache = new Cache();
        }


        public bool Add<T>(string key, T item, int expireInMinute)
        {
            try
            {
                if (item == null || string.IsNullOrEmpty(key)) return false;

                HttpRuntime.Cache.Insert(key, item, null, DateTime.Now.AddMinutes(expireInMinute), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Remove(string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key)) return true;
                HttpRuntime.Cache.Remove(key);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public T Get<T>(string key)
        {
            try
            {
                object data = HttpRuntime.Cache[key];
                if (null != data)
                {
                    try
                    {
                        return (T)data;
                    }
                    catch
                    {
                        return default(T);
                    }
                }
                else
                {
                    return default(T);
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public static CachedEnum.CachedTypes Key
        {
            get { return CachedEnum.CachedTypes.IIS; }
        }

        public void Dispose()
        {
            // Do nothing
        }
    }
}
