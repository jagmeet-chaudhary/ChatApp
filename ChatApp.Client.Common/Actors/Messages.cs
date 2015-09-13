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
        public class SetPrompt
        {
            public SetPrompt(string prompt)
            {
                Prompt = prompt;
            }
            public string Prompt { get; private set; }
        }
        #region Success message
        public class InputSuccess
        {
            public InputSuccess(string reason)
            {
                Reason = reason;
            }
            public string Reason { get; private set; }

        }
        #endregion

        #region Error Message
        /// <summary>
        /// Base class for signalling that user input was invalid.
        /// </summary>
        public class InputError
        {
            public InputError(string reason)
            {
                Reason = reason;
            }

            public string Reason { get; private set; }
        }

        /// <summary>
        /// User provided blank input.
        /// </summary>
        public class NullInputError : InputError
        {
            public NullInputError(string reason) : base(reason) { }
        }

        /// <summary>
        /// User provided invalid input (currently, input w/ odd # chars)
        /// </summary>
        public class ValidationError : InputError
        {
            public ValidationError(string reason) : base(reason) { }
        }
        #endregion

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
            public TryInitializeChat (string from,string to)
	        {
                From = from;To = to;
	        }
            public string From { get; private set; }
            public string To { get; private set; }

        }
        public class StartChat
        {
            public IActorRef Server { get; private set; }
            public StartChat(IActorRef server)
            {
                Server = server;
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
            public IActorRef ConsoleActor { get; set; }

            public AttachConsole(IActorRef consoleActor)
            {
                ConsoleActor = consoleActor;
            }
        }
        public class StatusCheck
        {
        }
       
    }

    public enum UserState
    {
        Offline,
        Online,
    };
}
