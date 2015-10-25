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
        }
        public IActorRef ActorReference { get; set; }
        
    }
    public class ChatServerCoordinatorActor : ReceiveActor
    {
        List<string> _registeredUsers;
        Dictionary<string, ChatUserReference> _userAddresses;
        Dictionary<Guid, IActorRef> _chatSessions;

        public ChatServerCoordinatorActor()
        {
            _userAddresses = new Dictionary<string, ChatUserReference>();
            _chatSessions = new Dictionary<Guid, IActorRef>();
            _registeredUsers = new List<string>();
            Become(InitializeServer);
            //Become(Testing);
            ConsoleActorContainer.Instance.WriterActor.Tell("Server started...");
            
        }

        private void Testing()
        {
        }
        private void InitializeServer()
        {
            Receive<Messages.RegisterUser>(x => HandleRegisterUser(x));
            Receive<Messages.TryInitializeChat>(x => HandleTryInitializeChat(x));
            Receive<Terminated>(x => HandleTerminated(x));

            Receive<Messages.ChangeState>(x => x.State == UserState.Online, x => HandleStateOnline(x));
            Receive<Messages.ChangeState>(x => x.State == UserState.Offline, x => HandleStateOffline(x));
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

        private void HandleTryInitializeChat(Messages.TryInitializeChat tryInitializeChatMsg)
        {
             if (_userAddresses.Keys.Contains(tryInitializeChatMsg.From))
            {
                IActorRef chatServerActor = null;
                Guid sessionId = Guid.Empty;
                if (tryInitializeChatMsg.SessionId == Guid.Empty)
                {
                    chatServerActor = Context.ActorOf(Props.Create(() =>
                          new ChatServerActor()));

                    sessionId = Guid.NewGuid();
                    _chatSessions.Add(sessionId, chatServerActor);
                    _userAddresses[tryInitializeChatMsg.From].ActorReference.Tell(new Messages.StartChat(sessionId,chatServerActor));

                }
                else
                {
                    chatServerActor = _chatSessions[tryInitializeChatMsg.SessionId];
                    sessionId = tryInitializeChatMsg.SessionId;
                }
                 

                tryInitializeChatMsg.UserList.ForEach(x =>
                {
                    if(_userAddresses.Keys.Contains(x))
                    {
                        _userAddresses[x].ActorReference.Tell(new Messages.StartChat(sessionId,chatServerActor));
                    }
                    else
                    {
                        _userAddresses[tryInitializeChatMsg.From].ActorReference.Tell(
                            new Messages.StatusMessage(string.Format("Cannot add {0} to the chat as user is offline.",x),StatusMessageType.Error));
                    }
                }); 
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

