namespace TodoList.Commands;

public class SyncCommand : ICommand
{
	public bool Pull { get; set; } = false;
	public bool Push { get; set; } = false;

	public void Execute()
	{
		if (!Pull && !Push)
		{
			Console.WriteLine("Используйте: sync --pull или sync --push");
			return;
		}

		try
		{
			using var testClient = new HttpClient();
			testClient.Timeout = TimeSpan.FromSeconds(5);
			var testResponse = testClient.GetAsync("http://localhost:5000/profiles").Result;
		}
		catch (Exception)
		{
			Console.WriteLine("Ошибка: сервер недоступен.");
			return;
		}

		var apiStorage = new ApiDataStorage();

		if (Pull)
		{
			try
			{
				var profiles = apiStorage.LoadProfiles().ToList();
				if (profiles.Any())
				{
					AppInfo.Profiles = profiles;
				}

				foreach (var profile in AppInfo.Profiles)
				{
					var todos = apiStorage.LoadTodos(profile.Id).ToList();
					AppInfo.UserTodos[profile.Id] = todos;

					if (AppInfo.CurrentProfileId == profile.Id)
					{
						var currentTodos = AppInfo.GetCurrentTodos();
						if (Program.CurrentTodoList != null)
						{
							for (int i = Program.CurrentTodoList.Count - 1; i >= 0; i--)
							{
								Program.CurrentTodoList.Delete(i);
							}
							foreach (var todo in currentTodos)
							{
								Program.CurrentTodoList.Add(todo);
							}
						}
					}
				}

				Console.WriteLine("Данные получены с сервера.");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка при получении данных: {ex.Message}");
			}
		}
		else if (Push)
		{
			try
			{
				apiStorage.SaveProfiles(AppInfo.Profiles);

				foreach (var kvp in AppInfo.UserTodos)
				{
					apiStorage.SaveTodos(kvp.Key, kvp.Value);
				}

				Console.WriteLine("Данные отправлены на сервер.");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка при отправке данных: {ex.Message}");
			}
		}
	}

	public void Unexecute() { }
}