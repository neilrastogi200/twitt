namespace Twitter2
{
    public interface IUserRepository
    {
        void CreateUser(User user);
        User GetUsers(string userName);
    }
}