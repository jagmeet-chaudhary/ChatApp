using Akka.Actor;
using ChatApp.Client.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Common
{
    public class ConsoleReaderActor : UntypedActor
    {
        private string _prompt;
         protected override void OnReceive(object message)
        {
            if (message is Messages.ContinueProcessing)
            {

                ConsoleActorContainer.Instance.WriterActor.Tell(string.Format("{0}>", _prompt));
                var command = Console.ReadLine();
                ActorSystemContainer.Instance.System.ActorSelection(string.Format("user/{0}", ActorNames.ClientCoordinatorActor))
                                               .Tell(new Messages.ConsoleCommand(command));
                Self.Tell(new Messages.ContinueProcessing());
            }
            if(message is Messages.SetPrompt)
            {
                _prompt = ((Messages.SetPrompt)message).Prompt;
                Self.Tell(new Messages.ContinueProcessing());
            }

        }

    }
}


