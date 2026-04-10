using TodoList.Models;
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
				if (item.ProfileId == Guid.Empty)
					item.ProfileId = userId;
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

	public static async Task SyncWithApiAsync(bool pull = false, bool push = false)
	{
		var apiStorage = new ApiDataStorage();

		if (pull)
		{
			var profiles = apiStorage.LoadProfiles().ToList();
		}
		else if (push)
		{
			apiStorage.SaveProfiles(AppInfo.Profiles);
			foreach (var kvp in AppInfo.UserTodos)
			{
				apiStorage.SaveTodos(kvp.Key, kvp.Value);
			}
		}
	}


}