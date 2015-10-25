using Akka.Actor;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Common
{
    public class ConsoleWriterActor : UntypedActor
    {
        Dictionary<StatusMessageType, ConsoleColor> messageColors = new Dictionary<StatusMessageType, ConsoleColor>();
        public ConsoleWriterActor()
        {
            messageColors.Add(StatusMessageType.Error, ConsoleColor.Red);
            messageColors.Add(StatusMessageType.Success, ConsoleColor.Green);
            messageColors.Add(StatusMessageType.Warning, ConsoleColor.Yellow);
            messageColors.Add(StatusMessageType.Info, ConsoleColor.Gray);
            messageColors.Add(StatusMessageType.None, ConsoleColor.White);

        }
        protected override void OnReceive(object message)
        {
            if(message is Messages.StatusMessage)
            {
                var statusMessage = message as Messages.StatusMessage;
                var color = messageColors[statusMessage.MessageType];
                Console.ForegroundColor = color;
                Console.WriteLine(statusMessage.Message);
                
            }
            else
            {
                Console.WriteLine(message);
            }
            Console.ResetColor();
        }
    }
}
