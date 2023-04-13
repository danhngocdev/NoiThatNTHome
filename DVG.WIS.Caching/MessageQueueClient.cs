using DVG.WIS.Caching.DTO.Entities;
using DVG.WIS.Utilities;
using ServiceStack.Redis;
using ServiceStack.Redis.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVG.WIS.Caching.Cached.CachedEnum;

namespace DVG.WIS.Caching
{
    public class MessageQueueClient
    {
        private static PooledRedisClientManager redisFactory;
        private RedisMqServer mqHost;

        private static MessageQueueClient _instance;
        private static object LockObject = new object();
        private static bool _isAllowUsingRedis = false;
        private string _redisIp = AppSettings.Instance.GetString("RedisIP");
        private int _redisPort = AppSettings.Instance.GetInt32("RedisPort");
        private int _redisDB = AppSettings.Instance.GetInt32("RedisDB");

        public MessageQueueClient()
        {
            _isAllowUsingRedis = AppSettings.Instance.GetBool("AllowRedisMQ");
            string redisIp = _redisIp;
            int redisPort = _redisPort;
            int redisDB = _redisDB;
            redisFactory = new PooledRedisClientManager(redisDB, string.Format("{0}:{1}", redisIp, redisPort));
            
            mqHost = new RedisMqServer(redisFactory);
        }

        public static MessageQueueClient Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (LockObject)
                    {
                        _instance = new MessageQueueClient();
                    }
                }
                return _instance;
            }
        }

        public void SendMessage<T>(T data, MessageTypeEnum messageType)
        {
            using (var mqClient = mqHost.CreateMessageQueueClient())
            {
                mqClient.Publish(new MessageSender { Data = data, MessageType = (int)messageType });
            }
        }
    }
}
