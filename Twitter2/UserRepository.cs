using System.Collections.Generic;
using System.Linq;

namespace Twitter2
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
    }
}
