using DVG.WIS.Utilities;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Caching.CacheQueue
{
    public enum RegisterEmailCacheType
    {
        AddEmail = 0,
        AddLog = 1
    }

    public class RegisterEmailCacheQueue: IDisposable
    {
        IRedisClient _redisClient;
        public const string RateQueueKeySuffix = "_RegisterEmailListItemQueueCache";

        private readonly string _listItemQueueKey;
        // Lock with acsynchrnous for update cache
        private static readonly object _objLock = new object();

        private static readonly object _redisLock = new object();

        public RegisterEmailCacheQueue(RegisterEmailCacheType emailCacheType)
        {
            _listItemQueueKey = emailCacheType.ToString() + RateQueueKeySuffix;
            lock (_redisLock)
            {
                try
                {
                    _redisClient = RedisCacheProvider.CreateInstance(RedisServerType.Sync);
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                }
            }
        }
        /// <summary>
        /// The enqueue item on list
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool EnqueueItem(string item)
        {
            if (!ValidateRedis()) return false;
            lock (_objLock)
            {
                _redisClient.EnqueueItemOnList(_listItemQueueKey, item);
            }
            return true;
        }
        /// <summary>
        /// The dequeue item from list
        /// </summary>
        /// <returns></returns>
        public string DequeueItem()
        {
            if (!ValidateRedis()) return null;
            lock (_objLock)
            {
                var result = _redisClient.DequeueItemFromList(_listItemQueueKey);
                return result;
            }
        }
        public void Dispose()
        {
            if (_redisClient != null)
            {
                _redisClient.Dispose();
                _redisClient = null;
            }
        }

        private bool ValidateRedis()
        {
            if (_redisClient == null)
            {
                Logger.WriteLog(Logger.LogType.Error,"[Redis] Connect redis failure!");
                return false;
            }

            return true;
        }
    }
}
