using Akka.Actor;
using ChatApp.Client.Common;
using ChatApp.Common;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatApp.Commmon
{
    public class ChatConsoleActor : UntypedActor
    {
        private string _userName;
        private string _clientActorPath;
        private IActorRef _clientActor;
        private IReceiver _receiver;
        CommandManager _commandManager;
        CommandFactory _commandFactory;
        private List<Char> buffer = new List<char>();
        public ChatConsoleActor(string userName,string clientActorPath)
        {
            
            _userName = userName;
            _clientActorPath = clientActorPath;

            _clientActor = ActorSystemContainer.Instance.System.ActorSelection(_clientActorPath).ResolveOne(new TimeSpan(0, 0, 10)).Result;
            _clientActor.Tell(new Messages.AttachConsole(Self));
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "actor", _clientActor }, { "userName", _userName } };
            //Dictionary<string, object> parameters = new Dictionary<string, object>() { { "filePath", @"c:\logs\fileoutput.txt" } };
            _receiver = GlobalIocContainer.Container.IocContainer.Resolve<IReceiver>(parameters);
            _commandFactory = GlobalIocContainer.Container.IocContainer.Resolve<CommandFactory>();
            _commandManager = new CommandManager();
        }
        protected override void OnReceive(object message)
        {
            if (message is Messages.StatusMessage)
            {
                ConsoleActorContainer.Instance.WriterActor.Tell(message);
            }
            if (message is Messages.ContinueProcessing)
            {

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if(key.Modifiers == ConsoleModifiers.Control) 
                    {
                        switch(key.Key)
                        {
                            case ConsoleKey.Z:
                                _commandManager.Undo();
                                break;
                            case ConsoleKey.A:
                                Console.WriteLine("Enter Command:");
                                var command  = Console.ReadLine();
                                _clientActor.Tell(new Messages.ConsoleCommand(command));
                                break;
                        }
                        
                    }
                    else
                    {
                         var command = _commandFactory.CreateCommand(key, _receiver);
                         _commandManager.ExecuteCommand(command);
                    }
                    
                }
            }
            Self.Tell(new Messages.ContinueProcessing());
        }

    }
}
