namespace Twitter2.Commands
{
    public interface ICommandFactory
    {
        bool HandleCommand(string command);
    }
}