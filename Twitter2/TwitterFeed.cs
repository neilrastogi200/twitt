using System;
using System.Collections.Generic;
using System.Linq;

namespace Twitter2
{
    public class TwitterFeed : ITwitterFeed
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;

        public TwitterFeed(IMessageRepository messageRepository, IUserRepository userRepository)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
        }
            
        public void PublishMessage(string userName, string text)
        {
            if(!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(text))
            {
                User user = null;

               var findUser = _userRepository.GetUsers(userName);

                if (findUser == null)
                {
                   user = CreateUser(userName);
                }

            
                var message = new Message {Content = text, DateTime = DateTime.UtcNow,User = user};

                CreateMessage(message);

                if (user?.Messages == null)
                {
                    if (user != null)
                    {
                        user.Messages = new List<Message>() {message};
                    }
                }
                else
                {
                    user.Messages.Add(new Message() {Content = text,DateTime = DateTime.UtcNow});
                }

            }
        }

        private void CreateMessage(Message message)
        { 
            _messageRepository.CreateMessage(message);
        }

        public User CreateUser(string userName)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = userName
            };

            _userRepository.CreateUser(user);

            return user;
        }

        public void FollowUser(string userName, string follower)
        {
            var follow = _userRepository.GetUsers(userName);
            var following = _userRepository.GetUsers(follower);
        }

        public string ReadCommand(string userName)
        {
            var user =  _userRepository.GetUsers(userName);
            if (user == null)
            {
                throw new ArgumentException(nameof(userName),"bbbb");
            }

            if (user.Messages == null || !user.Messages.Any())
            {
                throw new ArgumentException(nameof(user.Messages));
            }

            string result = null;
            foreach (var message in user.Messages.OrderBy(x => x.DateTime))
            {
                result = message.Content + (DateTime.UtcNow  - message.DateTime).Minutes.ToString();
            }

            return result;
        }


        public ConsoleInput ParsingInput(string data)
        {
            data = data?.Trim();

            var Separator = ' ';
            
            if (string.IsNullOrWhiteSpace(data))
            {
                return null;
            }

            var parts = data.Split(new[] { Separator }, 3);

            var hasCommandText = parts.Length > 1;
            var hasData = parts.Length > 2;
            return new ConsoleInput
            {
                UserName = parts.FirstOrDefault(),
                Command = hasCommandText ? parts[1] : null,
                Mesage = hasData ? parts.Last() : null
            };
        }
    }
}
