﻿using System;
using System.Collections.Generic;
using System.Linq;
using Twitter2.Infrastructure;
using Twitter2.Models;
using Twitter2.Repository;

namespace Twitter2.Services
{
    public class TwitterUserFeedService : ITwitterUserFeedService
    {
        private readonly IConsole _console;
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;

        public TwitterUserFeedService(IMessageRepository messageRepository, IUserRepository userRepository,
            IConsole console)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _console = console;
        }

        public void PublishMessage(string userName, string text)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(text))
            {
                User user = null;

                var findUser = _userRepository.GetUsers(userName);

                if (findUser == null)
                {
                    user = CreateUser(userName);
                }

                var message = user != null
                    ? new Message {Content = text, DateTime = DateTime.UtcNow, User = user}
                    : new Message {Content = text, DateTime = DateTime.UtcNow, User = findUser};

                CreateMessage(message);

                if (user != null)
                {
                    if (user.Messages == null)
                    {
                        user.Messages = new List<Message> {message};
                    }
                    else
                    {
                        user.Messages.Add(new Message {Content = text, DateTime = DateTime.UtcNow});
                    }
                }
                else if (findUser != null)
                {
                    if (findUser.Messages == null)
                    {
                        findUser.Messages = new List<Message> {message};
                    }
                    else
                    {
                        findUser.Messages.Add(new Message {Content = text, DateTime = DateTime.UtcNow});
                    }
                }
            }
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

        public bool FollowUser(string userName, string follower)
        {
            var currentUser = _userRepository.GetUsers(userName);
            var followingUser = _userRepository.GetUsers(follower);

            if (currentUser == null || followingUser == null)
            {
                throw new ArgumentException("The users do not exist");
            }

            if (currentUser.Following != null && currentUser.Following.Any(x => x.UserName == follower))
            {
                return true;
            }

            if (currentUser.Following == null)
            {
                currentUser.Following = new List<User> {followingUser};
                var result = _userRepository.Update(currentUser);
            }
            else
            {
                currentUser.Following.Add(followingUser);
            }

            return true;
        }

        public string ReadMessage(string userName)
        {
            var user = _userRepository.GetUsers(userName);
            if (user == null)
            {
                throw new ArgumentException(nameof(userName), "bbbb");
            }

            if (user.Messages == null || !user.Messages.Any())
            {
                throw new ArgumentException(nameof(user.Messages));
            }

            foreach (var message in user.Messages.OrderBy(x => x.DateTime))
            {
                _console.WriteLine("{0} ({1} minutes ago)",
                    message.Content,
                    (DateTime.UtcNow - message.DateTime).Minutes);
            }

            return null;
        }

        public bool ShowWall(string userName)
        {
            var user = _userRepository.GetUsers(userName);

            //Listing all the messages, including the user following another user.

            if (user == null)
            {
                throw new ArgumentException(nameof(userName), "bbbb");
            }

            var currentUserMessages = user.Messages.Select(x => new {user.UserName, Message = x}).ToList();

            if (user.Following != null)
            {
                foreach (var item in user.Following)
                {
                    foreach (var message in item.Messages)
                    {
                        currentUserMessages.Add(new {item.UserName, Message = message});
                    }
                }
            }

            foreach (var message in currentUserMessages)
            {
                _console.WriteLine("{0} - {1} ({2} minutes ago)",
                    message.UserName,
                    message.Message.Content,
                    (DateTime.UtcNow - message.Message.DateTime).Minutes);
            }

            return true;
        }

        private void CreateMessage(Message message)
        {
            _messageRepository.CreateMessage(message);
        }
    }
}