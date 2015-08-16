using Akka.Actor;
using Akka.Configuration;
using ChatApp.Client;
using ChatApp.Client.Common;
using ChatApp.Server.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ChatApp.Actors
{
    public class ChatClientCoordinatorActor : ReceiveActor
    {
        private readonly  string userName = "Jags";
        private ActorSelection _chatCoordinatorActor;
        private IActorRef _consoleReaderActor;
        private IActorRef _consoleWriterActor;
        private IActorRef _serverActor;
        public ChatClientCoordinatorActor()
        {
          
            //get a reference to the remote actor
            _chatCoordinatorActor = ClientActorSystemContainer.Instance.System.ActorSelection("akka.tcp://ChatServer@localhost:8080/user/ChatApp");
            _consoleReaderActor = ClientActorSystemContainer.Instance.System.ActorOf(Props.Create(() => new ConsoleReaderActor()));
            _consoleWriterActor = ClientActorSystemContainer.Instance.System.ActorOf(Props.Create(() => new ConsoleWriterActor()));

            Receive<Messages.Ok>(x => HandleOK(x));
            Receive<Messages.StartChat>(x => HandleStartChat(x));
            
            Receive<Messages.TryInitializeChat>(x => HandleTryInitializeChat(x));
            Receive<Messages.ConsoleCommand>(x => HandleConsoleCommand(x));

            _chatCoordinatorActor.Tell(new ChatApp.Actors.Messages.ChangeState(UserState.Online, userName));
            _consoleReaderActor.Tell("Start");
            
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
            _consoleWriterActor.Tell(new Messages.InputSuccess("Connected chat..."));
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
