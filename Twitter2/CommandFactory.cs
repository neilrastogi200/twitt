using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitter2
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
