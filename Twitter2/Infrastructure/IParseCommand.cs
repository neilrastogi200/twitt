using Twitter2.Models;

namespace Twitter2.Infrastructure
{
    public interface IParseCommand
    {
        ConsoleInput ParsingInput(string data);
    }
}
