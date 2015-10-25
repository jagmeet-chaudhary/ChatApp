using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Common
{

    public class Messages
    {
        public class StatusMessage
        {
            public StatusMessageType MessageType { get; private set; }
            public string Message { get; private set; }

            public StatusMessage(string _message,StatusMessageType _messageType)
            {
                Message = _message; MessageType = _messageType;
            }

        }
        public class SetPrompt
        {
            public SetPrompt(string prompt)
            {
                Prompt = prompt;
            }
            public string Prompt { get; private set; }
        }
        public class ContinueProcessing { }

        public class ConsoleCommand
        {
            public string Command { get; private set; }
            public ConsoleCommand(string command)
            {
                Command = command;
            }
        }
        public class Ping
        {
            public string Message { get; private set; }
            public Ping(string message)
            {
                Message = message;
            }
        }   
        public class Pong
        {
            public string Message { get; private set; }
            public Pong(string message)
            {
                Message = message;
            }
        }
        public class RegisterUser
        {
            public string UserName { get; private set; }
            public RegisterUser (string userName)
	        {
                UserName = userName;
	        }
        }

        public class TryInitializeChat
        {
            public Guid SessionId { get; private set; }
            public List<String> UserList { get; private set; }
            public string From { get; private set; }
            public TryInitializeChat (Guid _sessionId,string _from,List<string> _userList)
	        {
                SessionId = _sessionId;
                UserList = _userList;
                From = _from;
	        }
        }
        public class StartChat
        {
            public Guid SessionId { get; private set; }
            public IActorRef Server { get; private set; }
            public StartChat(Guid _sessionId,IActorRef _server)
            {
                Server = _server;
                SessionId = _sessionId;
            }
        }
        public class AddToChat
        {
            public Dictionary<string,IActorRef> ParticipantList {get;private set ;}
            public AddToChat(Dictionary<string, IActorRef> participantList)
            {
                ParticipantList = participantList;
            }
        }
        public class Ok 
        {
        }

        public class ChangeState
        {
            public UserState State { get;private set; }
            public string UserName { get; private set; }
            public ChangeState(UserState state,string userName)
            {
                State = state;
                UserName = userName;
            }
        }
        public class ChatMessage
        {
            public String From { get; private set; }
            public string Message { get; private set; }
            public ChatMessage(string from ,string message)
            {
                Message  = message;
                From = from;
            }
        }
        public class AttachConsole
        {
            public IActorRef ConsoleActor { get;  private set; }

            public AttachConsole(IActorRef consoleActor)
            {
                ConsoleActor = consoleActor;
            }
        }
        public class InvitePeople
        {
            public Guid SessionId { get; private set; }
            public List<string> UserList { get; private set; }
            public InvitePeople(List<string> userList,Guid sessionId)
            {
                UserList = userList;
                SessionId = sessionId;
            }
        }
       
    }

    public enum UserState
    {
        Offline,
        Online,
    };
    public enum StatusMessageType
    {
        None,
        Error,
        Info,
        Warning,
        Success
    }
}
