﻿using System;
using Twitter2.Infrastructure;
using Twitter2.Models;
using Twitter2.Services;

namespace Twitter2.Commands
{
    public class FollowCommand : ICommand
    {
        private readonly IParseCommand _parseCommand;
        private readonly ITwitterUserFeedService _twitterUserFeedService;

        public FollowCommand(ITwitterUserFeedService twitterUserFeedService, IParseCommand parseCommand)
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
                _twitterUserFeedService.FollowUser(parameters.UserName, parameters.Mesage);
                return true;
            }

            return false;
        }

        private ConsoleInput CanHandle(string element)
        {
            var parameters = _parseCommand.ParsingInput(element);

            if (parameters.Command == "follows")
            {
                return parameters;
            }

            return null;
        }
    }
}