using Akka.Actor;
using ChatApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Client
{
    public class ChatClientActor : ReceiveActor
    {
        private IActorRef _serverActor;
        public ChatClientActor(IActorRef serverActor)
        {
            _serverActor = serverActor;
            Receive<Messages.ChatMessage>(x => HandleChatMessage(x));

        }
        private void HandleChatMessage(Messages.ChatMessage chatMessageMsg)
        {
            _serverActor.Tell(chatMessageMsg);
        }
    }
}