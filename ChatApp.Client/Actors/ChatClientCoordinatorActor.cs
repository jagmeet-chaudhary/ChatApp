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
namespace ChatApp.Client
{
    public class ChatClientCoordinatorActor : ReceiveActor
    {
        private string userName;
        private ActorSelection _chatServerCoordinatorActor;
        
        private IActorRef _serverActor;
        public ChatClientCoordinatorActor()
        {

            
            
            
            Become(LoggedOut);
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
            ConsoleActorContainer.Instance.ReaderActor.Tell(new Messages.SetPrompt(string.Format("LoggedIn:{0}",userName)));
            Receive<Messages.Ok>(x => HandleOK(x));
            Receive<Messages.StartChat>(x => HandleStartChat(x));
            Receive<Messages.Ping>(x => HandlePing(x));
            Receive<Messages.ConsoleCommand>(x => HandleConsoleCommand(x));

            //Conosle Command messages
            Receive<ConsoleCommandMessages.StartChatCommandMessage>(x => HandleStartChatCommand(x));
            Receive<ConsoleCommandMessages.LogOutCommandMessage>(x => HandleLogOutCommand(x));
            
        }

        private void HandleLogOutCommand(ConsoleCommandMessages.LogOutCommandMessage x)
        {
            userName = string.Empty;
            _chatServerCoordinatorActor.Tell(new Messages.ChangeState(UserState.Offline, userName));
            Become(LoggedOut);
        }
        private void HandleLoginAsCommand(ConsoleCommandMessages.LoginAsCommandMessage loginAsCmdMsg)
        {
            userName = loginAsCmdMsg.UserName;
            _chatServerCoordinatorActor.Tell(new Messages.ChangeState(UserState.Online, userName));
            Become(LoggedIn);
        }

        private void  HandleStartChatCommand(ConsoleCommandMessages.StartChatCommandMessage startChatCommandMsg)
        {

            _chatServerCoordinatorActor.Ask<Messages.StartChat>(new Messages.TryInitializeChat(userName, startChatCommandMsg.UserName))
                .PipeTo<Messages.StartChat>(Self);
        }

        private void HandlePing(Messages.Ping x)
        {

            _chatServerCoordinatorActor.Tell(new Messages.Pong(userName));
        }
        protected override void PostStop()
        {
            ConsoleActorContainer.Instance.WriterActor.Tell(new Messages.InputSuccess("Stopping...."));
           
            base.PostStop();
        }
        
      
        protected override void PreStart()
        {
            //get a reference to the remote actor
            _chatServerCoordinatorActor = ActorSystemContainer.Instance.System.ActorSelection("akka.tcp://ChatAppServer@localhost:8080/user/ChatApp");
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
                ConsoleActorContainer.Instance.WriterActor.Tell(new Messages.InputError(ex.Message));
                
            }
            
        }
        private void HandleStartChat(Messages.StartChat startChatMsg)
        {
            _serverActor = startChatMsg.Server;
            var chatProcess = new Process();
            chatProcess.StartInfo.FileName = @"C:\Users\jagmeet.jag-richi\Documents\Git\ChatApp.Console\ChatApp.Console\bin\Debug\ChatApp.Console.exe";
            chatProcess.StartInfo.Arguments = string.Format("StartChat -u {0}", userName);
            chatProcess.StartInfo.UseShellExecute = true;
            chatProcess.Start();
            
        }

        private void HandleOK(Messages.Ok ok)
        {
            
        }




        
    }
}
