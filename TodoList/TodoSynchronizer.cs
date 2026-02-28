namespace TodoList;

public static class TodoSynchronizer
{
	private static FileManager _fileManager;

	public static void Initialize(FileManager fileManager)
	{
		_fileManager = fileManager;
	}

	public static void SyncWithAppInfo(TodoList todoList)
	{
		if (AppInfo.CurrentProfileId.HasValue)
		{
			var userId = AppInfo.CurrentProfileId.Value;
			var updatedTodos = new List<TodoItem>();
			foreach (var item in todoList)
			{
				updatedTodos.Add(item);
			}

			AppInfo.UserTodos[userId] = updatedTodos;

			try
			{
				_fileManager.SaveTodos(userId, updatedTodos);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка сохранения задач: {ex.Message}");
			}
		}
	}
}