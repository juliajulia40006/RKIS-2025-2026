using System;
using TodoList.Commands;
using TodoList;
using System.IO;

class Program
{
	private static TodoList.TodoList currentTodoList;

	static void Main(string[] args)
	{
		Console.WriteLine("The program was made by Deinega and Piyagova");

		string dataDirectory = "data";
		FileManager.EnsureDataDirectory(dataDirectory);
		string profilesFilePath = Path.Combine(dataDirectory, "profiles.csv");
		FileManager.LoadProfiles(profilesFilePath);
		Console.WriteLine("Войти в существующий профиль? [y/n]");
		string answer = Console.ReadLine()?.Trim().ToLower();

		if (answer == "y")
		{
			if (!LoginUser())
				return;
		}
		else if (answer == "n")
		{
			if (!RegisterUser())
				return;
		}
		else
		{
			Console.WriteLine("Неверный выбор.");
			return;
		}

		InitializeTodoList();

		while (true)
		{
			Console.Write("\nВведите команду (help - список команд): ");
			string input = Console.ReadLine()?.Trim() ?? "";

			if (string.IsNullOrEmpty(input))
				continue;
			var currentTodos = AppInfo.GetCurrentTodos();
			var currentProfile = AppInfo.CurrentProfile;

			ICommand command = CommandParser.Parse(input, currentTodos, currentProfile);

			if (command is ExitCommand)
			{
				SaveAllData();
				command.Execute();
				break;
			}

			if (command != null)
			{
				if (command is UndoCommand || command is RedoCommand)
				{
					command.Execute();
				}
				else if (command is HelpCommand || command is ViewCommand ||
						 command is ReadCommand || command is ProfileCommand)
				{
					command.Execute();
				}
				else
				{
					command.Execute();
					AppInfo.undoStack.Push(command);
					AppInfo.redoStack.Clear();
				}
			}
		}
	}

	private static void InitializeTodoList()
	{
		if (!AppInfo.CurrentProfileId.HasValue) return;
		currentTodoList = new TodoList.TodoList();

		currentTodoList.OnTodoAdded += FileManager.SaveTodoList;
		currentTodoList.OnTodoDeleted += FileManager.SaveTodoList;
		currentTodoList.OnTodoUpdated += FileManager.SaveTodoList;
		currentTodoList.OnStatusChanged += FileManager.SaveTodoList;

		var currentTodos = AppInfo.GetCurrentTodos();
	}

	private static bool LoginUser()
	{
		Console.Write("Логин: ");
		string login = Console.ReadLine()?.Trim();

		Console.Write("Пароль: ");
		string password = Console.ReadLine()?.Trim();

		var profile = AppInfo.Profiles.FirstOrDefault(p =>
			p.Login.Equals(login, StringComparison.OrdinalIgnoreCase) &&
			p.Password == password);

		if (profile != null)
		{
			AppInfo.CurrentProfileId = profile.Id;
			AppInfo.CurrentProfile = profile;

			if (!AppInfo.UserTodos.ContainsKey(profile.Id))
			{
				AppInfo.UserTodos[profile.Id] = AppInfo.LoadUserTodos(profile.Id);
			}

			Console.WriteLine($"Добро пожаловать, {profile.GetInfo()}!");
			AppInfo.undoStack.Clear();
			AppInfo.redoStack.Clear();

			InitializeTodoList();
			return true;
		}

		Console.WriteLine("Неверный логин или пароль.");
		return false;
	}

	private static bool RegisterUser()
	{
		Console.Write("Логин: ");
		string login = Console.ReadLine()?.Trim();

		if (AppInfo.Profiles.Any(p => p.Login.Equals(login, StringComparison.OrdinalIgnoreCase)))
		{
			Console.WriteLine("Этот логин уже занят.");
			return false;
		}

		Console.Write("Пароль: ");
		string password = Console.ReadLine()?.Trim();

		Console.Write("Имя: ");
		string firstName = Console.ReadLine()?.Trim();

		Console.Write("Фамилия: ");
		string lastName = Console.ReadLine()?.Trim();

		Console.Write("Год рождения: ");
		if (!int.TryParse(Console.ReadLine(), out int birthYear))
		{
			Console.WriteLine("Неверный год рождения.");
			return false;
		}

		Guid id = Guid.NewGuid();
		var profile = new Profile(id, login, password, firstName, lastName, birthYear);

		AppInfo.Profiles.Add(profile);
		AppInfo.CurrentProfileId = id;
		AppInfo.CurrentProfile = profile;
		AppInfo.UserTodos[id] = new List<TodoItem>();
		string profilesFilePath = Path.Combine("data", "profiles.csv");
		FileManager.SaveProfiles(profilesFilePath);

		Console.WriteLine($"Профиль создан! Добро пожаловать, {profile.GetInfo()}!");

		InitializeTodoList();
		return true;
	}

	private static void SaveAllData()
	{
		string profilesFilePath = Path.Combine("data", "profiles.csv");
		FileManager.SaveProfiles(profilesFilePath);
		foreach (var kvp in AppInfo.UserTodos)
		{
			AppInfo.SaveUserTodos(kvp.Key);
		}
	}

}

   