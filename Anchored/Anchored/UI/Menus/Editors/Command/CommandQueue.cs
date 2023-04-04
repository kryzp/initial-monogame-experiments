using Anchored.State;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anchored.UI.Menus.Editors.Command
{
	public class CommandQueue
	{
		private Stack<ICommand> commands = new Stack<ICommand>();
		private Stack<ICommand> redo = new Stack<ICommand>();

		public Editor Editor;

		public void Do(ICommand command)
		{
			redo.Clear();

			commands.Push(command);
			command.Do(Editor.Map);
		}

		public void Undo()
		{
			if (commands.Count == 0)
				return;

			var command = commands.Pop();

			command.Undo(Editor.Map);
			redo.Push(command);
		}

		public void Redo()
		{
			if (redo.Count == 0)
				return;

			var command = redo.Pop();

			command.Do(Editor.Map);
			commands.Push(command);
		}
	}
}
