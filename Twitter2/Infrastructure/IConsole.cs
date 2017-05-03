using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitter2.Infrastructure
{
    public interface IConsole
    {
        string ReadLine();

        void Write(string line);

        void WriteLine(string line);

        void WriteLine(string line, params object[] args);
    }
}
