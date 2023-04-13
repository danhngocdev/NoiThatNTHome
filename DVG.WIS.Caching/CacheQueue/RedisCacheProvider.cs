using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Redis;
using DVG.WIS.Utilities;

namespace DVG.WIS.Caching.CacheQueue
{
    public class RedisServerInfo
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public int Db { get; set; }
    }

    public enum RedisServerType
    {
        Common,
        Sync,
        Online
    }

    public class RedisCacheProvider
    {
        private static Dictionary<int, RedisServerInfo> _dictRedisServer;
        private const string RedisServerSuffix = "RedisServer";
        private static object redisObj = new object();
        static RedisCacheProvider()
        {
            _dictRedisServer = new Dictionary<int, RedisServerInfo>();
        }

        public static IRedisClient CreateInstance(string host, int port, int db = 0)
        {
            var redis = new RedisClient(host, port)
            {
                Db = db
            };

            return redis;
        }

        public static IRedisClient CreateInstance(RedisServerType redisServerType)
        {
            var redisInfo = GetRedisInfo(redisServerType);
            var redis = CreateInstance(redisInfo.Host, redisInfo.Port, redisInfo.Db);

            return redis;
        }

        public static IRedisNativeClient CreateNativeInstance(string host, int port, int db = 0)
        {
            var redis = new RedisNativeClient(host, port)
            {
                Db = db
            };

            return redis;
        }

        public static IRedisNativeClient CreateNativeInstance(RedisServerType redisServerType)
        {
            var redisInfo = GetRedisInfo(redisServerType);
            var redis = CreateNativeInstance(redisInfo.Host, redisInfo.Port, redisInfo.Db);
            return redis;
        }

        private static RedisServerInfo GetRedisInfo(RedisServerType redisServerType)
        {
            RedisServerInfo redisServerInfo;
            int intRedisServerType = (int)redisServerType;
            lock (redisObj)
            {
                if (_dictRedisServer.ContainsKey(intRedisServerType))
                {
                    redisServerInfo = _dictRedisServer[intRedisServerType];
                }
                else
                {
                    redisServerInfo = new RedisServerInfo();
                    redisServerInfo.Host = AppSettings.Instance.GetString("RedisIP");
                    redisServerInfo.Port = AppSettings.Instance.GetInt32("RedisPort");
                    redisServerInfo.Db = AppSettings.Instance.GetInt32("RedisDBQueue");

                    _dictRedisServer.Add(intRedisServerType, redisServerInfo);
                }
            }

            return redisServerInfo;
        }
    }
}
