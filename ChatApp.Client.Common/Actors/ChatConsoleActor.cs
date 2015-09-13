using Akka.Actor;
using ChatApp.Client.Common;
using ChatApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Commmon
{
    public class ChatConsoleActor : UntypedActor
    {
        private string _userName;
        private string _clientActorPath;
        public ChatConsoleActor(string userName,string clientActorPath)
        {
            _userName = userName;
            _clientActorPath = clientActorPath;

            ActorSystemContainer.Instance.System.ActorSelection(_clientActorPath).Tell(new Messages.AttachConsole(Self));
        }
        protected override void OnReceive(object message)
        {

            if (message is Messages.ContinueProcessing)
            {

                ConsoleActorContainer.Instance.WriterActor.Tell(string.Format("{0}>",_userName ));
                var command = System.Console.ReadLine();

                ActorSystemContainer.Instance.System.ActorSelection(_clientActorPath)
                                              .Tell(new Messages.ChatMessage(_userName, command));
                
                Self.Tell(new Messages.ContinueProcessing());
            }
            if(message is Messages.InputSuccess)
            {
                ConsoleActorContainer.Instance.WriterActor.Tell(((Messages.InputSuccess)message).Reason);
            }
        }

    }
}
