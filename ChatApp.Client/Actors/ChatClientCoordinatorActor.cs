using Akka.Actor;
using Akka.Configuration;
using ChatApp.Client;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Common;
namespace ChatApp.Client
{
    public class ChatClientCoordinatorActor : ReceiveActor
    {
        private readonly  string userName = "Jags";
        private ActorSelection _chatCoordinatorActor;
        
        private IActorRef _serverActor;
        public ChatClientCoordinatorActor()
        {

            
            Receive<Messages.Ok>(x => HandleOK(x));
            Receive<Messages.StartChat>(x => HandleStartChat(x));
            Receive<Messages.Ping>(x => HandlePing(x));
            Receive<Messages.TryInitializeChat>(x => HandleTryInitializeChat(x));
            Receive<Messages.ConsoleCommand>(x => HandleConsoleCommand(x));
        }

        private void HandlePing(Messages.Ping x)
        {
            _chatCoordinatorActor.Tell(new Messages.Pong(userName));
        }
        protected override void PostStop()
        {
            ConsoleActorContainer.Instance.WriterActor.Tell(new Messages.InputSuccess("Stopping...."));
            _chatCoordinatorActor.Tell(new Messages.ChangeState(UserState.Offline, userName));
            base.PostStop();
        }
        
      
        protected override void PreStart()
        {
            //get a reference to the remote actor
            _chatCoordinatorActor = ActorSystemContainer.Instance.System.ActorSelection("akka.tcp://ChatAppServer@localhost:8080/user/ChatApp");
            _chatCoordinatorActor.Tell(new Messages.ChangeState(UserState.Online, userName));
            base.PreStart();
        }
        private void HandleConsoleCommand(Messages.ConsoleCommand consoleCommandMsg)
        {
            var args = ProcessCommand(consoleCommandMsg.Command);
            CommandLineExecutor.InvokeCommand(args, typeof(ChatClientCoordinatorActor));
        }

        private void HandleTryInitializeChat(Messages.TryInitializeChat tryInitiailizeChatMsg)
        {
            _chatCoordinatorActor.Ask<Messages.StartChat>(new Messages.TryInitializeChat(tryInitiailizeChatMsg.From, tryInitiailizeChatMsg.To))
                .PipeTo<Messages.StartChat>(Self);
                    
        }
       

        private void HandleStartChat(Messages.StartChat startChatMsg)
        {
            _serverActor = startChatMsg.Server;
            ConsoleActorContainer.Instance.WriterActor.Tell(new Messages.InputSuccess("Connected chat..."));
        }

        private void HandleOK(Messages.Ok ok)
        {
            
        }
        private  string[] ProcessCommand(string command)
        {
            string[] splitCommands = command.Split(' ');
            return splitCommands.Select(x => x.Trim()).ToArray();
        }
        [Command("StartChat")]
        private  void StartChat([CommandParameter("u")]string userName)
        {

            Self.Tell(new Messages.TryInitializeChat(this.userName, userName));
        }

    }
}
