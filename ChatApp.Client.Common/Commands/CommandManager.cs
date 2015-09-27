using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Common
{
    public class CommandManager
    {
        private Stack<IUndoableCommand> _undoableCommands;
        public CommandManager()
        {
            _undoableCommands = new Stack<IUndoableCommand>();
        }
        public void ExecuteCommand(Command cmd)
        {
            if (cmd is IUndoableCommand) _undoableCommands.Push(cmd as IUndoableCommand);
            cmd.Execute();

        }
        public void Undo()
        {
            var command = _undoableCommands.Pop();
            command.Undo();

        }
    }
}
