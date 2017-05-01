using System;

namespace Twitter2
{
    public class Message
    {
        public string Content { get; set; }

        public DateTime DateTime { get; set; }

        public User User { get; set; }
    }
}