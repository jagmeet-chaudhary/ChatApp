using Akka.Actor;
using ChatApp.Client;
using ChatApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatApp.ChatConsole
{
    public class Commands
    {
        public static string[] ProcessCommand(string command)
        {
            string[] splitCommands = command.Split(' ');
           return  splitCommands.Select(x => x.Trim()).ToArray();
        }
        [Command("StartChat")]
        public static void StartChat([CommandParameter("u")]string userName)
        {

        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            //Come online
            var clientCoordinatorActor = ActorSystemContainer.Instance.System.ActorOf(Props.Create(() => new ChatClientCoordinatorActor()));
            ConsoleActorContainer.Instance.ReaderActor.Ask("Start");
            ActorSystemContainer.Instance.System.AwaitTermination();
            //var consoleReaderActor = ClientActorSystemContainer.Instance.System.ActorOf(Props.Create(() => new ConsoleReaderActor()));
            //clientCoordinatorActor.Tell("Start");
            //while(true)
            //{
            //    Console.WriteLine("Command>");
            //    var command = Console.ReadLine();

            //}
        }
    }
}
