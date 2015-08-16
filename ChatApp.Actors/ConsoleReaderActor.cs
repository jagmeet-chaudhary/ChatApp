using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Actors;

namespace ChatApp.Server.Actors
{
    public class ConsoleReaderActor : UntypedActor
    {
        public const string StartCommand = "Start";
        public const string ExitCommand = "Exit";

        protected override void OnReceive(object message)
        {
            if (message.Equals(StartCommand))
            {
                Console.WriteLine("Command>");
            }
            var command = Console.ReadLine();
            Sender.Tell(new Messages.ConsoleCommand(command));

        }

    }
}


