using System.Linq;
using Twitter2.Models;

namespace Twitter2.Infrastructure
{
    public class ParseCommand :IParseCommand
    {
        public ConsoleInput ParsingInput(string data)
        {
            data = data?.Trim();

            var Separator = ' ';

            if (string.IsNullOrWhiteSpace(data))
            {
                return null;
            }

            var parts = data.Split(new[] { Separator }, 3);

            var hasCommandText = parts.Length > 1;
            var hasData = parts.Length > 2;
            return new ConsoleInput
            {
                UserName = parts.FirstOrDefault(),
                Command = hasCommandText ? parts[1] : null,
                Mesage = hasData ? parts.Last() : null
            };
        }
    }
}
