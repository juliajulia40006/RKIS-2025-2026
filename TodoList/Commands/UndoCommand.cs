
namespace TodoList.Commands;
public class UndoCommand:ICommand
{
	public void Execute()
	{
		if (AppInfo.undoStack.Count > 0)
		{
			ICommand command = AppInfo.undoStack.Pop();
			command.Unexecute();
			AppInfo.redoStack.Push(command);
			Console.WriteLine("Команда отменена.");
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
