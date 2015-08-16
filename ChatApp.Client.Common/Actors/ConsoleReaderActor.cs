using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Common
{
    public class ConsoleReaderActor : UntypedActor
    {
        public const string StartCommand = "Start";
        public const string ExitCommand = "Exit";

        protected override void OnReceive(object message)
        {
            if (message.Equals(StartCommand))
            {
                ConsoleActorContainer.Instance.WriterActor.Tell("Command>");
            }
            var command = Console.ReadLine();
            Sender.Tell(new Messages.ConsoleCommand(command));

        }

    }
}


