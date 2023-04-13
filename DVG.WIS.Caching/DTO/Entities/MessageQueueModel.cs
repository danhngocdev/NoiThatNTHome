using System;

namespace DVG.WIS.Caching.DTO.Entities
{
    public abstract class MessageBase
    {
        public string Name { get; set; }
        public object Data { get; set; }
        public int MessageType { get; set; }
    }

    public class MessageSender : MessageBase
    {
        public MessageSender()
        {
            SentDate = DateTime.Now;
        }
        public DateTime SentDate { get; set; }
    }

    public class MessageReceiver : MessageBase
    {
        public DateTime ReceiverDate { get; set; }
        public DateTime SentDate { get; set; }
    }
}
