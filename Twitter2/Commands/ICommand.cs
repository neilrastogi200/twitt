using Twitter2.Models;

namespace Twitter2.Commands
{
    public interface ICommand
    { 
        bool ExecuteCommand(string data);

        //ConsoleInput CanHandle(string element);
    }
}
