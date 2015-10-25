using Akka.Actor;
using Akka.Configuration;
using ChatApp.Client;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Common;
using System.Diagnostics;
using ChatApp.Client.Common;
using System.Configuration;
using System.IO;

namespace ChatApp.Client
{
    public class ChatClientCoordinatorActor : ReceiveActor
    {
        private string _userName;
        private ActorSelection _chatServerCoordinatorActor;
        Dictionary<Guid, IActorRef> clientActors = new Dictionary<Guid, IActorRef>();
        //private IActorRef _serverActor;
        public ChatClientCoordinatorActor()
        {
            
            Become(LoggedOut);
            //Become(Testing);
            ConsoleActorContainer.Instance.ReaderActor.Tell(new Messages.ContinueProcessing());
        }
        private void LoggedOut()
        {
            ConsoleActorContainer.Instance.ReaderActor.Tell(new Messages.SetPrompt("LoggedOut"));
            Receive<ConsoleCommandMessages.LoginAsCommandMessage>(x => HandleLoginAsCommand(x));
            Receive<Messages.ConsoleCommand>(x => HandleConsoleCommand(x));
            //ReceiveAny(x => ConsoleActorContainer.Instance.WriterActor.Tell(new Messages.InputError("Please login.")));
        }
        private void LoggedIn()
        {
            ConsoleActorContainer.Instance.ReaderActor.Tell(new Messages.SetPrompt(string.Format("LoggedIn:{0}",_userName)));
            Receive<Messages.Ok>(x => HandleOK(x));
            Receive<Messages.StartChat>(x => HandleStartChat(x));
            Receive<Messages.Ping>(x => HandlePing(x));
            Receive<Messages.ConsoleCommand>(x => HandleConsoleCommand(x));
            Receive<Messages.StatusMessage>(x => HandleStatusMessage(x));
            Receive<Messages.InvitePeople>(x => HandleInvitePeople(x));
            //Conosle Command messages
            Receive<ConsoleCommandMessages.StartChatCommandMessage>(x => HandleStartChatCommand(x));
            Receive<ConsoleCommandMessages.LogOutCommandMessage>(x => HandleLogOutCommand(x));
            
        }

        private void HandleInvitePeople(Messages.InvitePeople invitePeopleMsg)
        {
            _chatServerCoordinatorActor.Tell(new Messages.TryInitializeChat(invitePeopleMsg.SessionId,_userName, invitePeopleMsg.UserList));
        }

        private void HandleStatusMessage(Messages.StatusMessage statusMessageMsg)
        {
            ConsoleActorContainer.Instance.WriterActor.Tell(statusMessageMsg);
        }
        private void HandleLogOutCommand(ConsoleCommandMessages.LogOutCommandMessage x)
        {
            _userName = string.Empty;
            _chatServerCoordinatorActor.Tell(new Messages.ChangeState(UserState.Offline, _userName));
            Become(LoggedOut);
        }
        private void HandleLoginAsCommand(ConsoleCommandMessages.LoginAsCommandMessage loginAsCmdMsg)
        {
            _userName = loginAsCmdMsg.UserName;
            _chatServerCoordinatorActor.Tell(new Messages.ChangeState(UserState.Online, _userName));
            Become(LoggedIn);
        }
        private void  HandleStartChatCommand(ConsoleCommandMessages.StartChatCommandMessage startChatCommandMsg)
        {
            _chatServerCoordinatorActor.Ask<Messages.StartChat>(new Messages.TryInitializeChat(Guid.Empty,_userName, new List<string>() { startChatCommandMsg.UserName }))
                .PipeTo<Messages.StartChat>(Self);
        }
        private void HandlePing(Messages.Ping x)
        {
            Console.WriteLine("received : " + x.Message);
        }
        protected override void PostStop()
        {
            ConsoleActorContainer.Instance.WriterActor.Tell(new Messages.StatusMessage("Stopping...", StatusMessageType.Info));
            base.PostStop();
        }
        protected override void PreStart()
        {
            //get a reference to the remote actor
            _chatServerCoordinatorActor = ActorSystemContainer.Instance.System.ActorSelection(ConfigurationManager.AppSettings["ServerAddress"]);
            base.PreStart();
        }
        private void HandleConsoleCommand(Messages.ConsoleCommand consoleCommandMsg)
        {
            object message = null;
            try
            {
                message = consoleCommandMsg.Command.ToMessageType();
                Self.Tell(message);
            }
            catch (InvalidCommandException ex)
            {
                ConsoleActorContainer.Instance.WriterActor.Tell(new Messages.StatusMessage(ex.Message,StatusMessageType.Error));
                
            }
            
        }
        private void HandleStartChat(Messages.StartChat startChatMsg)
        {
            
            IActorRef clientActor = Context.ActorOf(Props.Create<ChatClientActor>(startChatMsg.Server, _userName, startChatMsg.SessionId), _userName);
            var remoteAddress = ((ExtendedActorSystem)ActorSystemContainer.Instance.System).Provider.DefaultAddress;
            var clientActorFullRemoteAddress = clientActor.Path.ToStringWithAddress(remoteAddress);
            clientActors.Add(startChatMsg.SessionId, clientActor);

            var chatProcess = new Process();
            chatProcess.StartInfo.FileName = @"ChatApp.Console.exe";
            chatProcess.StartInfo.UseShellExecute = true;
            chatProcess.StartInfo.Arguments = string.Format("{0} {1}", _userName, clientActorFullRemoteAddress);
            chatProcess.Start();
        }
        private void Testing()
        {

            Context.ActorOf(Props.Create<ChatClientActor>(Self, "Jags"), "Jags");
            var dkjd = ((ExtendedActorSystem)ActorSystemContainer.Instance.System).Provider.DefaultAddress;
            var path = string.Format(@"{0}/{1}", Self.Path.ToStringWithAddress(dkjd), "Jags");

            var chatProcess = new Process();
            chatProcess.StartInfo.FileName = @"C:\Users\jagmeet.jag-richi\Documents\Git\ChatApp.Console\ChatApp.Console\bin\Debug\ChatApp.Console.exe";
            chatProcess.StartInfo.UseShellExecute = true;
            chatProcess.StartInfo.Arguments = string.Format("{0} {1}", "Jags", path);
            chatProcess.Start();
        }
        private void HandleOK(Messages.Ok ok)
        {
            
        }




        
    }
}
