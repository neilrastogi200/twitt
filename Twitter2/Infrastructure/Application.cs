using System;

namespace Twitter2.Infrastructure
{
    public class Application : IApplication
    {
        private readonly ICommandFactory _commandFactory;

        public Application(ICommandFactory commandFactory)
        {
            _commandFactory = commandFactory;
        }

        public int Run()
        {
            string command = null;
            while (command != "exit")
            {
                if (!string.IsNullOrWhiteSpace(command))
                {
                    var parameters = _commandFactory.Handle(command);
                }

                Console.Write("> ");
                command = Console.ReadLine();

            }

            return 0;
        }
    }
}
