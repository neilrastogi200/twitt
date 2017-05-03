using Twitter2.Models;

namespace Twitter2.Services
{
    public interface ITwitterUserFeedService
    {
        User CreateUser(string userName);
        bool FollowUser(string userName, string follower);
        void PublishMessage(string userName, string text);
        string ReadMessage(string userName);
        bool ShowWall(string userName);
    }
}