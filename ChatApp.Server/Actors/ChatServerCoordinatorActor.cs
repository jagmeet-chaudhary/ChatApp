using Akka.Actor;
using ChatApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Server
{
    public class ChatUserReference
    {
        public ChatUserReference()
        {
            StatusCheckRetryCounter = 3;
        }
        public IActorRef ActorReference { get; set; }
        public int StatusCheckRetryCounter { get; set; }
    }
    public class ChatServerCoordinatorActor : ReceiveActor
    {
        List<string> _registeredUsers;
        Dictionary<string, ChatUserReference> _userAddresses;

        public ChatServerCoordinatorActor()
        {
            _userAddresses = new Dictionary<string, ChatUserReference>();
            _registeredUsers = new List<string>();
            Receive<Messages.RegisterUser>(x => HandleRegisterUser(x));
            Receive<Messages.TryInitializeChat>(x => HandleTryInitializeChat(x));
            Receive<Terminated>(x => HandleTerminated(x));

            Receive<Messages.ChangeState>(x => x.State == UserState.Online, x => HandleStateOnline(x));
            Receive<Messages.ChangeState>(x => x.State == UserState.Offline, x => HandleStateOffline(x));
            ConsoleActorContainer.Instance.WriterActor.Tell("Server started...");
            
        }

        private void HandleTerminated(Terminated terminatedMsg)
        {
            foreach(var userAddress in _userAddresses)
            {
                if(userAddress.Value.ActorReference.Path == terminatedMsg.ActorRef.Path)
                {
                    Self.Tell(new Messages.ChangeState(UserState.Offline, userAddress.Key));
                    break;
                }

            }
        }

       
        private void HandleStateOffline(Messages.ChangeState x)
        {
            if (_userAddresses.Keys.Contains(x.UserName))
            {
                _userAddresses.Remove(x.UserName);
                ConsoleActorContainer.Instance.WriterActor.Tell(string.Format("{0} is offline now.", x.UserName));
            }
        }

        private void HandleStateOnline(Messages.ChangeState x)
        {
            if (!_userAddresses.Keys.Contains(x.UserName))
            {
                Context.Watch(Sender);
                _userAddresses.Add(x.UserName, new ChatUserReference() { ActorReference = Sender});
            }
            ConsoleActorContainer.Instance.WriterActor.Tell(string.Format("{0} is online now.", x.UserName));
        }

        private void  HandlePing(Messages.Ping x)
        {
            Console.Write(x.Message);
        }

        private void HandleTryInitializeChat(Messages.TryInitializeChat x)
        {
            Dictionary<string, IActorRef> participantList = new Dictionary<string, IActorRef>();
                if(_userAddresses.Keys.Contains(x.To))
                    participantList.Add(x.To,_userAddresses[x.To].ActorReference);
             if(_userAddresses.Keys.Contains(x.From))
                    participantList.Add(x.To,_userAddresses[x.From].ActorReference);
            if(participantList.Count>1)
            {
                var chatServerActor = Context.ActorOf(Props.Create(() =>
                      new ChatServerActor()));
                var addToChat = new Messages.AddToChat(participantList);
                chatServerActor.Tell(addToChat);
                Sender.Tell(new Messages.StartChat(chatServerActor));
            }
              
     

        }


        private void HandleRegisterUser(Messages.RegisterUser x)
        {
            if(!string.IsNullOrEmpty(x.UserName))
            {
                _registeredUsers.Add(x.UserName);
            }
        }


        
    }
}

