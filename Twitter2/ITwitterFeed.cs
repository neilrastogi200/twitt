namespace Twitter2
{
    public interface ITwitterFeed
    {
        User CreateUser(string userName);
        void FollowUser(string userName, string follower);
        ConsoleInput ParsingInput(string data);
        void PublishMessage(string userName, string text);
    }
}