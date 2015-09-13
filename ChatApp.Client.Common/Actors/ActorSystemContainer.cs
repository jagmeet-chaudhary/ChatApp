using Akka.Actor;
using Akka.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ChatApp.Common
{
    public class ActorSystemContainer : IDisposable
    {
        private static ActorSystemContainer container;
        private ActorSystem actorSystem;
//        const string configValue = @"
//            akka {
//                actor {
//                    provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
//                }
//                remote {
//                    helios.tcp {
//                        port = 8080
//                        hostname = localhost
//                    }
//                }
//            }";
        public ActorSystem System { get { return actorSystem; } }
        private ActorSystemContainer()
        {

            var actorSystemName = ConfigurationManager.AppSettings["ActorSystemName"];
            actorSystem = ExtendedActorSystem.Create(actorSystemName);
        }
        public static ActorSystemContainer Instance
        {
            get
            {
                if(container == null)
                {
                    container = new ActorSystemContainer();
                }
                return container;
            }
        }

        public void Dispose()
        {
            actorSystem.Dispose();
        }
    }

}
