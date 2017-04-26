using System;
using System.Collections.Generic;

namespace Twitter2
{
    public class User
    {
        public Guid Id;
        public string UserName { get; set; }
        public IList<Message> Messages { get; set; }
        public IList<User> Following { get; set; }
    }
}