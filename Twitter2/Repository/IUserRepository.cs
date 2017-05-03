using Twitter2.Models;

namespace Twitter2.Repository
{
    public interface IUserRepository
    {
        void CreateUser(User user);
        User GetUsers(string userName);
        bool Update(User item);
    }
}