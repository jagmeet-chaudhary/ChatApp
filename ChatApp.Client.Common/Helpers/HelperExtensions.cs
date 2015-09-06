using ChatApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Common
{
    public static class HelperExtensions
    {
        public static object ToMessageType(this string command)
        {
            var commandparam = CommandToMessageConverter.ParseCommand(command);
            return CommandToMessageConverter.ConvertToMessage(commandparam, typeof(ConsoleCommandMessages));
        }
    }
}
