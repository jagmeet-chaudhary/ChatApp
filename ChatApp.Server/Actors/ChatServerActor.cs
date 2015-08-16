﻿using Akka.Actor;
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
        IActorRef _consoleReaderActor;
        IActorRef _consoleWriterActor;
        public ChatServerActor()
        {
            _participants = new Dictionary<string, IActorRef>();
            _consoleReaderActor = ActorSystemContainer.Instance.System.ActorOf(Props.Create(() => new ConsoleReaderActor()));
            _consoleWriterActor = ActorSystemContainer.Instance.System.ActorOf(Props.Create(() => new ConsoleWriterActor()));
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

