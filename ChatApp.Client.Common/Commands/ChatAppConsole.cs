using Akka.Actor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Common
{
    /// <summary>
    /// Test receiver for commands
    /// </summary>
    public class LocalFile : IReceiver
    {
        List<char> _buffer = new List<char>();
        string _filePath;
        public LocalFile(string filePath)
        {
            _filePath = filePath;
        }
        public char RemoveCharacter()
        {
            char c = '\0';
            if (_buffer.Count > 0)
            {
                c = _buffer[_buffer.Count - 1];
                _buffer.RemoveAt(_buffer.Count - 1);
                Console.Write("\b \b");
            }
            return c;
        }

        public void InputCharacter(char c)
        {
            _buffer.Add(c);
            Console.Write(c);
        }

        public void Submit()
        {
            var inputString = new string(_buffer.ToArray<char>());
            _buffer.Clear();
            File.AppendAllText(_filePath,inputString);
            File.AppendAllText(_filePath,Environment.NewLine);
            Console.WriteLine();
        }
    }
    public class ChatAppConsole : IReceiver
    {
        List<char> _buffer = new List<char>();
        IActorRef _actor;
        string _userName;
        
        public ChatAppConsole(string userName,IActorRef actor)
        {
            _actor = actor;
            _userName = userName;
        }
        public char RemoveCharacter()
        {
            char c = '\0';
            if (_buffer.Count > 0)
            {
                c = _buffer[_buffer.Count - 1];
                _buffer.RemoveAt(_buffer.Count - 1);
                Console.Write("\b \b");
            }
            return c;
        }

        public void InputCharacter(char c)
        {
            _buffer.Add(c);
            Console.Write(c);
        }

        public void Submit()
        {

            var inputString = new string(_buffer.ToArray<char>());
            _buffer.Clear();
            Console.WriteLine();
            _actor.Tell(new Messages.ChatMessage(_userName,inputString));
 
        }

    }
}
