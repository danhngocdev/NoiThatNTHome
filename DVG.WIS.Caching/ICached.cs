using System;

namespace DVG.WIS.Caching.Cached
{
    public interface ICached
    {
        bool Add<T>(string key, T item, int expireInMinute = 0);
        bool Remove(string key);
        T Get<T>(string key);
    }
}
