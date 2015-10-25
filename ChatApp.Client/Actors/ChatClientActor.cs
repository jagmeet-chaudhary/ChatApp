using Akka.Actor;
using ChatApp.Commmon;
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
        Guid _sessionId;
        public ChatClientActor(IActorRef serverActor,String UserName,Guid sessionId)
        {
            _sessionId = sessionId;
            _serverActor = serverActor;
            _userName = UserName;
            Receive<Messages.ChatMessage>(x => HandleChatMessage(x));
            Receive<Messages.Ping>(x => HandlePing(x));
            Receive<Messages.AttachConsole>(x => HandleAttachConsole(x));
            Receive<Messages.ConsoleCommand>(x => HandleConsoleCommand(x));
            Receive<ConsoleCommandMessages.InvitePeopleCommandMessage>(x => HandleInvitePeopleCommandMessage(x));
            var userList = new Dictionary<string,IActorRef>();
            userList.Add(UserName,Self);
            _serverActor.Tell(new Messages.AddToChat(userList));

        }

        private void HandleInvitePeopleCommandMessage(ConsoleCommandMessages.InvitePeopleCommandMessage invitePeopleCommandMsg)
        {
            var userList = invitePeopleCommandMsg.UserList.Trim().Trim(',').Split(',').Select(x => x);
            Context.Parent.Tell(new Messages.InvitePeople(userList.ToList(), _sessionId));
        }

        private void HandleConsoleCommand(Messages.ConsoleCommand consoleCommandMessage)
        {
            object message = null;
            try
            {
                message = consoleCommandMessage.Command.ToMessageType();
                Self.Tell(message);
            }
            catch (InvalidCommandException ex)
            {
                ConsoleActorContainer.Instance.WriterActor.Tell(new Messages.StatusMessage(ex.Message, StatusMessageType.Error));

            }
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
               _consoleActor.Tell(new Messages.StatusMessage(chatMessageMsg.Message,StatusMessageType.Success));
        }
    }
}