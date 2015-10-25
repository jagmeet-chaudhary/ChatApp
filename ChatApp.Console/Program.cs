using Akka.Actor;
using Akka.Configuration;
using ChatApp.Client.Common;
using ChatApp.Commmon;
using ChatApp.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Console
{
    class Program
    {

        static void Main(string[] args)
        {
           var userName = args[0];
           var path = args[1];
            var _chatConsoleActor = ActorSystemContainer.Instance.System.ActorOf(Props.Create(
                () => new ChatConsoleActor(userName,path)),string.Format("ChatConsole_{0}",userName)
                );
            _chatConsoleActor.Tell(new Messages.ContinueProcessing());
            ActorSystemContainer.Instance.System.AwaitTermination();
        }


    }
}
