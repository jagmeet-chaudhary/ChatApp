using Akka.Actor;
using Akka.Configuration;
using ChatApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatApp.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var chatServerActor = ActorSystemContainer.Instance.System.ActorOf(Props.Create(() => new ChatServerCoordinatorActor()),"ChatApp");
            Console.ReadLine();
            
        }
    }
}
