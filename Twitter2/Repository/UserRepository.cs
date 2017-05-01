﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Twitter2.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IList<User> _users;

        public UserRepository()
        {
            _users = new List<User>();
        }

        public User GetUsers(string userName)
        {
            
            return _users.FirstOrDefault(x => x.UserName == userName);
        }

        public void CreateUser(User user)
        {
            _users.Add(user);
        }

        public bool Update(User item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("This product does not exist");
            }

            var index = _users.Where(x => x.UserName == item.UserName);

            //if (index == -1)
            //{
            //    return false;
            //}

            foreach (var updatedItem in _users)
            {
                if (updatedItem.Id == item.Id)
                {
                    updatedItem.Following = item.Following;
                }
            }

            return true;
        }
    }
}
