namespace Twitter2.Commands
{
    public interface ICommandFactory
    {
        bool Handle(string command);
    }
}