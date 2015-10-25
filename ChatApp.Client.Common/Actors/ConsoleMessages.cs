using ChatApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Common
{
    public class ConsoleCommandMessages
    {
        [Command("StartChat")]
        public class StartChatCommandMessage
        {
            [CommandParameter("u")]
            public string UserName { get; private set; }
        }
        [Command("LoginAs")]
        public class LoginAsCommandMessage
        {
            [CommandParameter("u")]
            public string UserName { get; private set; }
        }
        [Command("LogOut")]
        public class LogOutCommandMessage
        {
        }
        [Command("InvitePeople")]
        public class InvitePeopleCommandMessage
        {
            [CommandParameter("ul")]
            public string UserList { get; private set; }
        }
    }
}
