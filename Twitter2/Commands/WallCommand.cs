using System;
using Twitter2.Infrastructure;
using Twitter2.Models;
using Twitter2.Services;

namespace Twitter2.Commands
{
    public class WallCommand : ICommand
    {
        private readonly IParseCommand _parseCommand;
        private readonly ITwitterUserFeedService _twitterUserFeedService;

        public WallCommand(ITwitterUserFeedService twitterUserFeedService, IParseCommand parseCommand)
        {
            _twitterUserFeedService = twitterUserFeedService;
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
                _twitterUserFeedService.ShowWall(parameters.UserName);
                return true;
            }

            return false;
        }

        private ConsoleInput CanHandle(string element)
        {
            var parameters = _parseCommand.ParsingInput(element);

            if (parameters.Command == "wall")
            {
                return parameters;
            }

            return null;
        }
    }
}