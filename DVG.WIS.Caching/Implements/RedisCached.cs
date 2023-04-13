using System;
using ServiceStack.Redis;
using DVG.WIS.Utilities;
using DVG.WIS.Caching.DTO.Entities;
using System.Web;

namespace DVG.WIS.Caching.Cached.Implements
{
    public class RedisCached : ICached, IRedisCached
    {
        private CachingConfigModel _configuration;

        public RedisCached()
        {
            this._configuration = new CachingConfigModel()
            {
                IpServer = AppSettings.Instance.GetString("RedisIP"),
                Port = AppSettings.Instance.GetInt32("RedisPort"),
                DB = AppSettings.Instance.GetInt32("RedisDB"),
                ConnectTimeout = AppSettings.Instance.GetInt32("RedisTimeout", 600),
                RedisSlotNameInMemory = AppSettings.Instance.GetString("RedisSlotName", "RedisTinxe")
            };
        }

        /// <summary>
        /// Initials Redis caching with configuration
        /// </summary>
        /// <exception cref="Configuration not allow null"></exception>
        /// <exception cref="Server's IP is undefined"></exception>
        /// <exception cref="Server's Port is undefined"></exception>
        /// <param name="configuration"></param>
        public RedisCached(CachingConfigModel configuration)
        {
            if (configuration.ConnectTimeout > 0)
                configuration.ConnectTimeout = configuration.ConnectTimeout;


            this._configuration = configuration;
        }

        private IRedisClient CreateInstance()
        {
            IRedisClient client;

            try
            {
                HttpContext context = HttpContext.Current;

                if (context != null)
                {
                    if (!context.Items.Contains(_configuration.RedisSlotNameInMemory))
                    {
                        client = new RedisClient(_configuration.IpServer, _configuration.Port)
                        {
                            Db = _configuration.DB,
                            ConnectTimeout = _configuration.ConnectTimeout
                        };

                        context.Items.Add(_configuration.RedisSlotNameInMemory, client);
                    }

                    return (RedisClient)context.Items[_configuration.RedisSlotNameInMemory];
                }
                else
                {
                    client = new RedisClient(_configuration.IpServer, _configuration.Port)
                    {
                        Db = _configuration.DB,
                        ConnectTimeout = _configuration.ConnectTimeout
                    };
                }

            }
            catch (Exception)
            {
                client = new RedisClient(_configuration.IpServer, _configuration.Port)
                {
                    Db = _configuration.DB,
                    ConnectTimeout = _configuration.ConnectTimeout
                };
            }

            return client;
        }

        public bool Add<T>(string key, T item, int expireInMinute = 0)
        {
            bool result = false;

            try
            {
                using (IRedisClient client = CreateInstance())
                {
                    if (expireInMinute > 0)
                        result = client.Set<T>(key, item, DateTime.Now.AddMinutes(expireInMinute));
                    else
                        result = client.Set<T>(key, item);
                }
            }
            catch (Exception ex)
            {
                Logger.TraceLog(string.Format("Set<T> Key: {0} {1} {2}", key, Environment.NewLine, ex.ToString()));
                return false;
            }

            return result;
        }

        public bool Remove(string key)
        {
            bool result = false;

            try
            {
                using (IRedisClient client = CreateInstance())
                {
                    result = client.Remove(key);
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);

            }

            return result;
        }

        public T Get<T>(string key)
        {
            T result = default(T);
            try
            {
                using (IRedisClient client = CreateInstance())
                {
                    if (IsRequestClearCache())
                    {
                        Remove(key);
                    }

                    result = client.Get<T>(key);
                }
            }
            catch (Exception ex)
            {
                Logger.TraceLog(string.Format("Set<T> Key: {0} {1} {2}", key, Environment.NewLine, ex.ToString()));
            }
            return result;
        }

        public void EndQueue(string key, string item, long score)
        {
            using (IRedisClient client = CreateInstance())
            {
                client.AddItemToSortedSet(key, item, score);
            }
        }

        public string DeQueue(string key)
        {
            using (IRedisClient client = CreateInstance())
            {
                return client.PopItemWithLowestScoreFromSortedSet(key);
            }
        }

        public void EndQueue<T>(string key, T item, long score)
        {
            try
            {
                if (score <= 0) score = Utils.DateTimeToUnixTime(DateTime.Now);
                string value = NewtonJson.Serialize(item);

                EndQueue(key, value, score);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
            }
        }

        public T DeQueue<T>(string key)
        {
            try
            {
                string value = DeQueue(key);
                if (!string.IsNullOrEmpty(value))
                {
                    return NewtonJson.Deserialize<T>(value);
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
            }

            return default(T);
        }


        public int GetSortedSetCount(string key)
        {
            using (IRedisClient client = CreateInstance())
            {
                return client.GetSortedSetCount(key);
            }
        }

        public void Push(string key, string item)
        {
            using (IRedisClient client = CreateInstance())
            {
                client.PushItemToList(key, item);
            }
        }

        public string Pop(string key)
        {
            using (IRedisClient client = CreateInstance())
            {
                return client.PopItemFromList(key);
            }
        }

        public void SetEntryOrIncrementValueInHash(string hashKey, string key)
        {
            using (IRedisClient client = CreateInstance())
            {
                if (!client.HashContainsEntry(hashKey, key))
                    client.SetEntryInHashIfNotExists(hashKey, key, "1");
                else
                    client.IncrementValueInHash(hashKey, key, 1);
            }
        }

        public System.Collections.Generic.Dictionary<string, string> GetAllEntriesAndRemoveFromHash(string hashkey)
        {
            using (IRedisClient client = CreateInstance())
            {
                var result = client.GetAllEntriesFromHash(hashkey);
                if (result != null) client.Remove(hashkey);
                return result;
            }
        }

        private bool IsRequestClearCache()
        {
            HttpContext context = HttpContext.Current;
            if (context.Request != null && context.Request.UserAgent != null)
            {
                return context.Request.UserAgent.Contains("refreshcache")
                    || context.Request.Headers["wis-refreshcache"] != null && context.Request.Headers["wis-refreshcache"] == "refreshcache";
            }
            return false;
        }
    }
}
