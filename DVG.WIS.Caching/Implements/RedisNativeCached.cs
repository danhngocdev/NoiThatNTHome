using System;
using ServiceStack.Redis;
using DVG.WIS.Utilities;
using DVG.WIS.Caching.DTO.Entities;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;

namespace DVG.WIS.Caching.Cached.Implements
{
    public class RedisNativeCached : ICached, IDisposable
    {
        private string clientIp = AppSettings.Instance.GetString("RedisNativeIP");
        private int clientPort = AppSettings.Instance.GetInt32("RedisNativePort");
        private int clientDb = AppSettings.Instance.GetInt32("RedisNativeDB");
        private int _connectTimeout = AppSettings.Instance.GetInt32("RedisNativeTimeout", 600);
        private static readonly string redisSlotName = AppSettings.Instance.GetString("RedisNativeSlotName", "RedisNativeTinxe");

        private CachingConfigModel _configuration;

        public RedisNativeCached()
        {
            this._configuration = new CachingConfigModel()
            {
                IpServer = clientIp,
                Port = clientPort,
                DB = clientDb,
                ConnectTimeout = _connectTimeout
            };
        }

        /// <summary>
        /// Initials Redis caching with configuration
        /// </summary>
        /// <exception cref="Configuration not allow null"></exception>
        /// <exception cref="Server's IP is undefined"></exception>
        /// <exception cref="Server's Port is undefined"></exception>
        /// <param name="configuration"></param>
        public RedisNativeCached(CachingConfigModel configuration)
        {
            if (configuration.ConnectTimeout > 0)
                configuration.ConnectTimeout = configuration.ConnectTimeout;


            this._configuration = configuration;
        }

        private IRedisNativeClient CreateInstance()
        {
            IRedisNativeClient client = new RedisNativeClient(_configuration.IpServer, _configuration.Port)
            {
                Db = _configuration.DB,
                ConnectTimeout = _configuration.ConnectTimeout
            };
            return client;
        }

        public bool Add<T>(string key, T item, int expireInMinute = 0)
        {
            try
            {
                using (IRedisNativeClient client = CreateInstance())
                {
                    if (expireInMinute > 0)
                        client.SetEx(key, expireInMinute * 60, ConvertTEntityToByte(item));
                    else
                        client.Set(key, ConvertTEntityToByte(item));
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
            }
            return false;
        }

        public bool Remove(string key)
        {
            try
            {
                using (IRedisNativeClient client = CreateInstance())
                {
                    return client.Del(key) > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);

                return false;
            }
        }

        public T Get<T>(string key)
        {
            try
            {
                using (IRedisNativeClient client = CreateInstance())
                {
                    if (IsRequestClearCache())
                    {
                        Remove(key);
                    }

                    byte[] bytes = client.Get(key);

                    T result = ConvertByteToTEntity<T>(bytes);

                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);

                return default(T);
            }
        }

        public void EndQueue(string key, string item, long score)
        {
            try
            {
                using (IRedisNativeClient client = CreateInstance())
                {
                    client.ZAdd(key, score, ConvertTEntityToByte(item));
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
            }
        }

        public string DeQueue(string key)
        {
            try
            {
                using (IRedisNativeClient client = CreateInstance())
                {
                    byte[] result = client.SPop(key);

                    return ConvertByteToTEntity<string>(result);
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
            }
            return string.Empty;
        }

        public void EndQueue<T>(string key, T item, long score)
        {
            try
            {
                using (IRedisNativeClient client = CreateInstance())
                {
                    if (score <= 0) score = Utils.DateTimeToUnixTime(DateTime.Now);

                    client.ZAdd(key, score, ConvertTEntityToByte(item));
                }
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
                using (IRedisNativeClient client = CreateInstance())
                {
                    byte[] result = client.SPop(key);

                    if (result != null)
                    {
                        return ConvertByteToTEntity<T>(result);
                    }
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
            try
            {
                using (IRedisNativeClient client = CreateInstance())
                {
                    return client.ZCard(key);
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
            }

            return 0;
        }

        public void Push(string key, string item)
        {
            try
            {
                using (IRedisNativeClient client = CreateInstance())
                {
                    client.LPush(key, ConvertTEntityToByte(item));
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
            }

        }

        public string Pop(string key)
        {
            try
            {
                using (IRedisNativeClient client = CreateInstance())
                {
                    byte[] result = client.RPop(key);

                    if (result != null)
                    {
                        return ConvertByteToTEntity<string>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
            }

            return string.Empty;
        }

        public void SetEntryOrIncrementValueInHash(string hashKey, string key)
        {
            throw new NotImplementedException();
        }

        public System.Collections.Generic.Dictionary<string, string> GetAllEntriesAndRemoveFromHash(string hashkey)
        {
            throw new NotImplementedException();
        }


        public static CachedEnum.CachedTypes Key
        {
            get { return CachedEnum.CachedTypes.Redis; }
        }

        private byte[] ConvertTEntityToByte<T>(T item)
        {
            if (item == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();

            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, item);
                return ms.ToArray();
            }
        }

        private T ConvertByteToTEntity<T>(byte[] bytes)
        {
            if (bytes == null)
                return default(T);

            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                object obj = bf.Deserialize(ms);
                return (T)obj;
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

        public void Dispose()
        {

        }
    }
}
