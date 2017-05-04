using System;
using Twitter2.Commands;

namespace Twitter2.Infrastructure
{
    public class Application : IApplication
    {
        private readonly ICommandFactory _commandFactory;
        private readonly IConsole _console;

        public Application(ICommandFactory commandFactory, IConsole console)
        {
            _commandFactory = commandFactory;
            _console = console;
        }

        public int Run()
        {
            string command = null;
            while (command != "exit")
            {
                if (!string.IsNullOrWhiteSpace(command))
                {
                    var parameters = _commandFactory.HandleCommand(command);
                }

                _console.Write("> ");
                command = _console.ReadLine();

            }

            return 0;
        }
    }
}
