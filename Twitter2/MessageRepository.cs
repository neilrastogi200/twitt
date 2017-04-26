using System.Collections.Generic;

namespace Twitter2
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IList<Message> _messages = new List<Message>();

        public void CreateMessage(Message message)
        {
            _messages.Add(message);
        }
    }
}
