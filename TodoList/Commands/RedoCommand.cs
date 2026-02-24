namespace TodoList.Commands;

public class RedoCommand : ICommand
{
	public void Execute()
	{
		if (AppInfo.redoStack.Count == 0)
			throw new InvalidOperationException("Нет команд для повтора.");

		ICommand command = AppInfo.redoStack.Pop();
		if (command is IUndo undoableCommand)
		{
			command.Execute();
			AppInfo.undoStack.Push(command);
			Console.WriteLine("Команда повторена.");
		}
		else
		{
			throw new InvalidOperationException("Эта команда не поддерживает повтор.");
		}
	}

	public void Unexecute()
	{

	}
}