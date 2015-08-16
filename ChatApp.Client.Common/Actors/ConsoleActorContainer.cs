using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Common
{
    public class ConsoleActorContainer
    {
        private static ConsoleActorContainer _container;
        private IActorRef _consoleReaderActor;
        private IActorRef _consoleWriterActor;
        public IActorRef ReaderActor { get { return _consoleReaderActor; } }
        public IActorRef WriterActor { get { return _consoleWriterActor; } }
        private ConsoleActorContainer()
        {
            _consoleReaderActor = ActorSystemContainer.Instance.System.ActorOf(Props.Create(() => new ConsoleReaderActor()));
            _consoleWriterActor = ActorSystemContainer.Instance.System.ActorOf(Props.Create(() => new ConsoleWriterActor())); 
        }
        public static ConsoleActorContainer Instance
        {
            get
            {
                if (_container == null)
                {
                    _container = new ConsoleActorContainer();
                }
                return _container;
            }
        }
    }
}
