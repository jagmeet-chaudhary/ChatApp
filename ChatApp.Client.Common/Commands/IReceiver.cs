using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Common
{
    public interface IReceiver
    {
        char RemoveCharacter();
        void InputCharacter(Char c);
        void Submit();


    }
}
