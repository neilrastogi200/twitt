using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twitter2.Infrastructure;

namespace Twitter2.Commands
{
    public class PostCommand : ICommand
    {
        private readonly ITwitterFeed _twitterFeed;
        private readonly IParseCommand _parseCommand;

        public PostCommand(ITwitterFeed twitterFeed, IParseCommand parseCommand)
        {
            _twitterFeed = twitterFeed;
            _parseCommand = parseCommand;
        }


        public bool ExecuteCommand(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentException("The argument data is null or empty");
            }

            var parameters = CanHandle(data);

            if (parameters != null)
            {
                _twitterFeed.PublishMessage(parameters.UserName, parameters.Mesage);
                return true;
            }

           

            return false;
        }


        public ConsoleInput CanHandle(string element)
        {
            var parameters = _parseCommand.ParsingInput(element);

            if (parameters.Command == "->")
            {
                return parameters;
            }

            return null;
        }
    }
}
