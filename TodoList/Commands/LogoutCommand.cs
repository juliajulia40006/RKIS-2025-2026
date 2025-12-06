namespace TodoList.Commands;

public class LogoutCommand : ICommand
{
	public void Execute()
	{
		if (AppInfo.CurrentProfileId.HasValue)
		{
			AppInfo.SaveUserTodos(AppInfo.CurrentProfileId.Value);
		}

		AppInfo.CurrentProfileId = null;
		AppInfo.CurrentProfile = null;

		AppInfo.undoStack.Clear();
		AppInfo.redoStack.Clear();

		Console.WriteLine("Вы вышли из профиля. Программа завершена.");
		Console.WriteLine("Перезапустите программу для входа в другой профиль.");
	}

	public void Unexecute() { }
}