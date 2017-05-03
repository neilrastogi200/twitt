using Twitter2.Models;

namespace Twitter2.Repository
{
    public interface IMessageRepository
    {
        void CreateMessage(Message message);
    }
}