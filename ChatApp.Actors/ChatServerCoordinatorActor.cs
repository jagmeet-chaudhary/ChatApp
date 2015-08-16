using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Actors;

namespace ChatApp.Actors
{
    
    public class ChatServerCoordinatorActor : ReceiveActor
    {
        List<string> registeredUsers;
        Dictionary<string, IActorRef> userAddresses;

        public ChatServerCoordinatorActor()
        {
            Console.WriteLine("Server started...");
            
            Receive<Messages.RegisterUser>(x => HandleRegisterUser(x));
            Receive<Messages.TryInitializeChat>(x => HandleTryInitializeChat(x));
            Receive<Messages.Ping>(x => HandlePing(x));
            Receive<Messages.ChangeState>(x => x.State==UserState.Online, x=>HandleStateOnline(x));
            Receive<Messages.ChangeState>(x => x.State == UserState.Offline, x => HandleStateOffline(x));
        }   

        private void HandleStateOffline(Messages.ChangeState x)
        {
            userAddresses.Remove(x.UserName);
        }

        private void HandleStateOnline(Messages.ChangeState x)
        {
            userAddresses.Add(x.UserName, Sender);
        }

        private void  HandlePing(Messages.Ping x)
        {
            Console.Write(x.Message);
        }

        private void HandleTryInitializeChat(Messages.TryInitializeChat x)
        {
            Dictionary<string, IActorRef> participantList = new Dictionary<string, IActorRef>();
                if(userAddresses.Keys.Contains(x.To))
                    participantList.Add(x.To,userAddresses[x.To]);
             if(userAddresses.Keys.Contains(x.From))
                    participantList.Add(x.To,userAddresses[x.From]);
            if(participantList.Count>1)
            {
                var chatServerActor = Context.ActorOf(Props.Create(() =>
                      new ChatServerActor()));
                var addToChat = new ChatApp.Actors.Messages.AddToChat(participantList);
                chatServerActor.Tell(addToChat);
                Sender.Tell(new Messages.StartChat(chatServerActor));
            }
              
     

        }


        private void HandleRegisterUser(Messages.RegisterUser x)
        {
            if(!string.IsNullOrEmpty(x.UserName))
            {
                registeredUsers.Add(x.UserName);
            }
        }


        
    }
}
