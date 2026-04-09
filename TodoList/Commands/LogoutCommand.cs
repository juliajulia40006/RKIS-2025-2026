namespace TodoList.Commands;

public class LogoutCommand : ICommand
{
	public void Execute()
	{
		if (AppInfo.CurrentProfileId.HasValue)
		{
			try
			{
				var userId = AppInfo.CurrentProfileId.Value;

				string dataDirectory = "data";
				var fileManager = new FileManager(dataDirectory);

				if (AppInfo.UserTodos.ContainsKey(userId))
				{
					var userTodos = AppInfo.UserTodos[userId];
					fileManager.SaveTodos(userId, userTodos);
					Console.WriteLine("Задачи сохранены.");
				}
				fileManager.SaveProfiles(AppInfo.Profiles);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка при сохранении данных: {ex.Message}");
			}
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