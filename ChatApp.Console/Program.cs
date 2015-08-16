using Akka.Actor;
using Akka.Configuration;
using ChatApp.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Console
{
    class ConsoleCommands
    {
        [Command("StartChat")]
        public static void StartChat([CommandParameter("u")]string userName)
        {
            System.Console.WriteLine("Starting chat with ..." + userName);
        }
        [Command("Exit")]
        public static void Exit()
        {
            Environment.Exit(0);
        }

        public static void ListCommands()
        {
            System.Console.WriteLine("Start chat with a user [ - StartChat -u {username}]");
            System.Console.WriteLine("Exit [ exit]");
        }
    }
 
    // ChatWith -u --username {Name}
    // exit -c --close
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                //args = new string[1];
                //args[0] = "Exit";
                //args[1] = "-u";
                //args[2] = "jagmeet";

                CommandLineExecutor.InvokeCommand(args, typeof(ConsoleCommands));
            }
            catch( InvalidCommandException iex)
            {
                System.Console.WriteLine(iex.Message);
                System.Console.WriteLine("List of valid commands:");
                ConsoleCommands.ListCommands();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Invalid Command");
                System.Console.WriteLine("List of valid commands:");
                ConsoleCommands.ListCommands();
                
            }
        }

    }
}
