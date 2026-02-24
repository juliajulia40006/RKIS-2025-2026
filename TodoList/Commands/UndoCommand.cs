namespace TodoList.Commands;
public class UndoCommand: ICommand
{
	public void Execute()
	{
		if (AppInfo.undoStack.Count == 0)
			throw new InvalidOperationException("Нет команд для отмены.");

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
			throw new InvalidOperationException("Эта команда не поддерживает отмену.");
		}
	}

	public void Unexecute()
	{

	}
}
