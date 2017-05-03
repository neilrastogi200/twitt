using System.Collections.Generic;

namespace Twitter2.Commands
{
    public class CommandFactory : ICommandFactory
    {
        private readonly ICollection<ICommand> _command;

        public CommandFactory(ICollection<ICommand> command)
        {
            _command = command;
        }

        public bool Handle(string command)
        {
            bool ok = false;

            foreach (var commandHandler in _command)
            {
                ok = commandHandler.ExecuteCommand(command);
                if (ok) break;
            }

            return ok;
        }
    }
}
