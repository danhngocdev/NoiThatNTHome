using System;

namespace DVG.WIS.Caching.Cached.Implements
{
    public class ElasticSearch : ICached
    {
        public ElasticSearch()
        {
        }
        public bool Add<T>(string key, T item, int expireInMinute)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }


        public static CachedEnum.CachedTypes Key
        {
            get { return CachedEnum.CachedTypes.ElasticSearch; }
        }
    }
}
