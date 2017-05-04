using System;
using Twitter2.Infrastructure;
using Twitter2.Models;
using Twitter2.Services;

namespace Twitter2.Commands
{
    public class ReadCommand : ICommand
    {
        private readonly IParseCommand _parseCommand;
        private readonly ITwitterUserFeedService _twitterUserFeedService;

        public ReadCommand(ITwitterUserFeedService twitterUserFeedService, IParseCommand parseCommand)
        {
            _twitterUserFeedService = twitterUserFeedService;
            _parseCommand = parseCommand;
        }

        public bool ExecuteCommand(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentException("The argument data is null or empty");
            }

            var parameters = CanHandleCommand(data);

            if (parameters != null)
            {
                var result =_twitterUserFeedService.ReadMessage(parameters.UserName);

                if (result == null)
                {
                    return true;
                }
            }


            return false;
        }

        private ConsoleInput CanHandleCommand(string element)
        {
            var parameters = _parseCommand.ParsingInput(element);

            if (!string.IsNullOrEmpty(parameters.UserName) && string.IsNullOrEmpty(parameters.Command))
            {
                return parameters;
            }

            return null;
        }
    }
}