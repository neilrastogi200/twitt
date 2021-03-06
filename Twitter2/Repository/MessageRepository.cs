﻿using System.Collections.Generic;
using Twitter2.Models;

namespace Twitter2.Repository
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
