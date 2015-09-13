using Akka.Actor;
using ChatApp.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Client
{
    public class ChatClientActor : ReceiveActor
    {
        private IActorRef _serverActor;
        private IActorRef _consoleActor;
        string _userName;
        public ChatClientActor(IActorRef serverActor,String UserName)
        {
            _serverActor = serverActor;
            _userName = UserName;
            Receive<Messages.ChatMessage>(x => HandleChatMessage(x));
            Receive<Messages.Ping>(x => HandlePing(x));
            Receive<Messages.AttachConsole>(x => HandleAttachConsole(x));
            var userList = new Dictionary<string,IActorRef>();
            userList.Add(UserName,Self);
            _serverActor.Tell(new Messages.AddToChat(userList));

        }

        private void HandleAttachConsole(Messages.AttachConsole attachConsoleMsg)
        {
            _consoleActor = attachConsoleMsg.ConsoleActor;
        }

        private void  HandlePing(Messages.Ping x)
        {
            Console.WriteLine("received : " + x.Message);
        }
        private void HandleChatMessage(Messages.ChatMessage chatMessageMsg)
        {
            if (chatMessageMsg.From == _userName)
                _serverActor.Tell(chatMessageMsg);
            else
               _consoleActor.Tell(new Messages.InputSuccess(chatMessageMsg.Message));
        }
    }
}