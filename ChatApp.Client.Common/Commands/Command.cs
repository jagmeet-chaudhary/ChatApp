using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Common
{
    #region Command Factory
    public abstract class CommandFactory
    {
        public abstract Command CreateCommand(ConsoleKeyInfo keyInfo, IReceiver receiver);
    }
    public class ChatCommandFactory : CommandFactory
    {
        public  override Command CreateCommand(ConsoleKeyInfo keyInfo, IReceiver receiver)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.Enter:
                    return new SubmitCommand(receiver);
                case ConsoleKey.Backspace:
                    return new BackspaceCommand(receiver);
                default:
                    return new InputCharacterCommand(keyInfo.KeyChar, receiver);
            }
        }
    } 
    #endregion

    #region Interfaces
    public interface IUndoableCommand
    {
        void Undo();
    } 
    #endregion

    #region Abstract command

    public abstract class Command
    {
        protected IReceiver _receiver;
        public abstract void Execute();
    }

    #endregion

    #region Concrete Commands
    public class InputCharacterCommand : Command, IUndoableCommand
    {

        private char _c;
        public InputCharacterCommand(Char c, IReceiver editor)
        {
            _c = c;
            _receiver = editor;
        }
        public override void Execute()
        {
            _receiver.InputCharacter(_c);
        }

        public void Undo()
        {
            _receiver.RemoveCharacter();
        }
    }
    public class SubmitCommand : Command
    {
        public SubmitCommand(IReceiver receiver)
        {
            _receiver = receiver;
        }
        public override void Execute()
        {
            _receiver.Submit();
        }
    }
    public class BackspaceCommand : Command, IUndoableCommand
    {
        char _previousChar;
        public BackspaceCommand(IReceiver receiver)
        {
            _receiver = receiver;
        }
        public override void Execute()
        {
            _previousChar = _receiver.RemoveCharacter();
        }

        public void Undo()
        {
            _receiver.InputCharacter(_previousChar);
        }
    }
    #endregion
}
