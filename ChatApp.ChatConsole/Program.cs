using Akka.Actor;
using ChatApp.Client;
using ChatApp.Client.Common;
using ChatApp.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
            FileStream fileStream = null;
            try
            {
                fileStream = new FileStream (Utility.GetErrorLogFileName(), FileMode.Append);
                Utility.RedirectStandardErrorToFile(fileStream);
           
                //Come online
                var clientCoordinatorActor = ActorSystemContainer.Instance.System.ActorOf(Props.Create(() => new ChatClientCoordinatorActor()), ActorNames.ClientCoordinatorActor);
                ActorSystemContainer.Instance.System.AwaitTermination();
            }
            finally
            {
                fileStream.Dispose();
            }

        }
    }
}
