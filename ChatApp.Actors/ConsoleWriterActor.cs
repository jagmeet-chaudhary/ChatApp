using Akka.Actor;
using ChatApp.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Server.Actors
{
    public class ConsoleWriterActor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            if (message is Messages.InputError)
            {

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine((message as Messages.InputError).Reason);
            }
            else if (message is Messages.InputSuccess)
            {

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine((message as Messages.InputSuccess).Reason);
            }
            else
            {
                Console.WriteLine(message);
            }
            Console.ResetColor();
        }
    }
}
