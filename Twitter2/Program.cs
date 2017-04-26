using System;

namespace Twitter2
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine(">");

            var read = Console.ReadLine();

            IMessageRepository messageRepository = new MessageRepository();
            IUserRepository userRepository = new UserRepository();

           TwitterFeed twitterFeed = new TwitterFeed(messageRepository,userRepository);
           var parameters = twitterFeed.ParsingInput(read);

            twitterFeed.PublishMessage(parameters.UserName, parameters.Mesage);

           var timeline = twitterFeed.ReadCommand(parameters.UserName);

            if (parameters.Command == "follows".ToLower())
            {
                twitterFeed.FollowUser(parameters.UserName, parameters.Mesage);
            }
        }
    }
}
