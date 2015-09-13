using Akka.Actor;
using ChatApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Server
{
    public class ChatServerActor : ReceiveActor
    {
        Dictionary<string, IActorRef> _participants;
        public ChatServerActor()
        {
            _participants = new Dictionary<string, IActorRef>();
            Receive<Messages.AddToChat>(x => HandleAddToChat(x));
            Receive<Messages.ChatMessage>(x => HandleChatMessage(x));
        }

        private void HandleChatMessage(Messages.ChatMessage chatMessage)
        {
            foreach(var participant in _participants.Where(x=>x.Key != chatMessage.From))
            {
                participant.Value.Tell(chatMessage);
            }
        }

        private void  HandleAddToChat(Messages.AddToChat addToChat)
        {
            foreach (var participant in addToChat.ParticipantList)
                _participants.Add(participant.Key, participant.Value);

        }


    }
}

