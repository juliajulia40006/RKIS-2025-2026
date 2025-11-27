
namespace TodoList.Commands;

public class RedoCommand : ICommand
{
	public void Execute()
	{
		if (AppInfo.redoStack.Count > 0)
		{
			ICommand command = AppInfo.redoStack.Pop();
			command.Execute();
			AppInfo.undoStack.Push(command);
			Console.WriteLine("Команда повторена.");
		}
		else
		{
			Console.WriteLine("Нет команд для повтора.");
		}
	}

	public void Unexecute()
	{

	}
}