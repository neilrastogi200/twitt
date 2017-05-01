using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitter2
{
    public interface ICommand
    {
        /// <summary>
        /// Execute Command and returns the output messages.
        /// </summary>
        /// <param name="data">Additional payload data. Not mandatory.</param>
        /// <returns>The output messages.</returns>
        bool ExecuteCommand(string data);

        ConsoleInput CanHandle(string element);
    }
}
