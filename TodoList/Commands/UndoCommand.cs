
namespace TodoList.Commands;
public class UndoCommand: ICommand
{
	public void Execute()
	{
		if (AppInfo.undoStack.Count > 0)
		{
			ICommand command = AppInfo.undoStack.Pop();
			if (command is IUndo undoableCommand)
			{
				undoableCommand.Unexecute();
				AppInfo.redoStack.Push(command);
				Console.WriteLine("Команда отменена.");
			}
			else
			{
				AppInfo.undoStack.Push(command);
				Console.WriteLine("Эта команда не поддерживает отмену.");
			}
		}
		else
		{
			Console.WriteLine("Нет команд для отмены.");
		}
	}

	public void Unexecute()
	{

	}
}
