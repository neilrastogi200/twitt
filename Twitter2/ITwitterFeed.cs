namespace Twitter2
{
    public interface ITwitterFeed
    {
        User CreateUser(string userName);
        bool FollowUser(string userName, string follower);
        void PublishMessage(string userName, string text);
        string ReadMessage(string userName);
        bool ShowWall(string userName);
    }
}