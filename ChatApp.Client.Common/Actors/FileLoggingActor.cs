using Akka.Actor;
using Akka.Event;
using ChatApp.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Common
{
    public class FileLoggingActor : UntypedActor
    {
        private FileStream _fileStream;
        private StreamWriter _streamWriter;
        public FileLoggingActor()
        {
            _fileStream = new FileStream(GetLogFileName() , FileMode.Append);
            _streamWriter = new StreamWriter(_fileStream);
        }

        protected override void OnReceive(object message)
        {
            if (message is InitializeLogger)
            {
                Sender.Tell(new LoggerInitialized());
            }
            else if(message is LogEvent)
            {
                _streamWriter.WriteAsync(message.ToString());
            }

        }
        private string GetLogFileName()
        {
            return FileNames.LogFilePrefix + "_" + DateTime.UtcNow.ToString("yyyyMMddHHmmssffff") + ".log";
        }
        protected override void PreRestart(Exception reason, object message)
        {
            _streamWriter.Dispose();
            _fileStream.Dispose();
            base.PreRestart(reason, message);
            
        }
    }

    public class FileLogger
    {
        public static FileLogger _logger;
        public  IActorRef _loggingActor;
        public IActorRef LoggingActor { get { return _loggingActor; } }
        public static FileLogger  Logger
        {
            get
            {
                if(_logger==null)
                {
                    _logger = new FileLogger();
                }
                return _logger;
            }
        }
        private FileLogger()
        {
            _loggingActor = ActorSystemContainer.Instance.System.ActorOf(Props.Create(() => new FileLoggingActor()));
        }
    }


   

}
