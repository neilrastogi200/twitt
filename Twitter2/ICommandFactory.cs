namespace Twitter2
{
    public interface ICommandFactory
    {
        bool Handle(string command);
    }
}