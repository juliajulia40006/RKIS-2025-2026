using TodoList;
using TodoList.Commands;
using TodoList.Exceptions;

class Program
{
	private static TodoList.TodoList currentTodoList;
	public static TodoList.TodoList CurrentTodoList => currentTodoList;
	private static FileManager _fileManager;

	static void Main(string[] args)
	{
		try
		{
			Console.WriteLine("The program was made by Deinega and Piyagova");

			string dataDirectory = "data";
			if (!Directory.Exists(dataDirectory))
			{
				Directory.CreateDirectory(dataDirectory);
			}

			_fileManager = new FileManager(dataDirectory);

			try
			{
				AppInfo.Profiles = _fileManager.LoadProfiles().ToList();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка загрузки профилей: {ex.Message}");
				AppInfo.Profiles = new List<Profile>();
			}

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
			CommandParser.Initialize(currentTodoList, AppInfo.CurrentProfile);

			while (true)
			{
				try
				{
					Console.Write("\nВведите команду (help - список команд): ");
					string input = Console.ReadLine()?.Trim() ?? "";

					if (string.IsNullOrEmpty(input))
						continue;

					ICommand command = CommandParser.Parse(input);

					if (command is ExitCommand)
					{
						SaveAllData();
						command.Execute();
						break;
					}

					if (command != null)
					{
						command.Execute();

						if (command is IUndo)
						{
							AppInfo.undoStack.Push(command);
							AppInfo.redoStack.Clear();
						}
					}
				}
				catch (TaskNotFoundException ex)
				{
					Console.WriteLine($"Ошибка задачи: {ex.Message}");
				}
				catch (AuthenticationException ex)
				{
					Console.WriteLine($"Ошибка авторизации: {ex.Message}");
				}
				catch (InvalidCommandException ex)
				{
					Console.WriteLine($"Ошибка команды: {ex.Message}");
				}
				catch (InvalidArgumentException ex)
				{
					Console.WriteLine($"Ошибка аргументов: {ex.Message}");
				}
				catch (DuplicateLoginException ex)
				{
					Console.WriteLine($"Ошибка регистрации: {ex.Message}");
				}
				catch (InvalidOperationException ex)
				{
					Console.WriteLine($"Ошибка операции: {ex.Message}");
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Критическая ошибка: {ex.Message}");
		}
	}

	private static void InitializeTodoList()
	{
		if (!AppInfo.CurrentProfileId.HasValue) return;
		var userTodos = AppInfo.GetCurrentTodos();
		currentTodoList = new TodoList.TodoList();
		foreach (var todo in userTodos)
		{
			currentTodoList.Add(todo);
		}

		var currentTodos = AppInfo.GetCurrentTodos();
	}

	private static bool LoginUser()
	{
		Console.Write("Логин: ");
		string login = Console.ReadLine()?.Trim();

		if (string.IsNullOrEmpty(login))
		{
			Console.WriteLine("Ошибка: Логин не может быть пустым.");
			return false;
		}

		Console.Write("Пароль: ");
		string password = Console.ReadLine()?.Trim();

		if (string.IsNullOrEmpty(password))
		{
			Console.WriteLine("Ошибка: Пароль не может быть пустым.");
			return false;
		}

		try
		{
			var profile = AppInfo.Profiles.FirstOrDefault(p =>
				p.Login.Equals(login, StringComparison.OrdinalIgnoreCase) &&
				p.Password == password);

			if (profile == null)
				throw new ProfileNotFoundException(login);

			AppInfo.CurrentProfileId = profile.Id;
			AppInfo.CurrentProfile = profile;

			if (!AppInfo.UserTodos.ContainsKey(profile.Id))
			{
				try
				{
					AppInfo.UserTodos[profile.Id] = _fileManager.LoadTodos(profile.Id).ToList();
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Ошибка загрузки задач: {ex.Message}");
					AppInfo.UserTodos[profile.Id] = new List<TodoItem>();

				}

			}

			Console.WriteLine($"Добро пожаловать, {profile.GetInfo()}!");
			AppInfo.undoStack.Clear();
			AppInfo.redoStack.Clear();

			InitializeTodoList();
			CommandParser.Initialize(currentTodoList, AppInfo.CurrentProfile);
			return true;
		}
		catch (AuthenticationException ex)
		{
			Console.WriteLine($"Ошибка: {ex.Message}");
			return false;
		}
	}

	private static bool RegisterUser()
	{
		Console.Write("Логин: ");
		string login = Console.ReadLine()?.Trim();

		if (string.IsNullOrEmpty(login))
		{
			Console.WriteLine("Ошибка: Логин не может быть пустым.");
			return false;
		}

		if (AppInfo.Profiles.Any(p => p.Login.Equals(login, StringComparison.OrdinalIgnoreCase)))
		{
			Console.WriteLine("Этот логин уже занят.");
			return false;
		}

		Console.Write("Пароль: ");
		string password = Console.ReadLine()?.Trim();

		if (string.IsNullOrEmpty(password))
		{
			Console.WriteLine("Ошибка: Пароль не может быть пустым.");
			return false;
		}

		Console.Write("Имя: ");
		string firstName = Console.ReadLine()?.Trim();

		if (string.IsNullOrEmpty(firstName))
		{
			Console.WriteLine("Ошибка: Имя не может быть пустым.");
			return false;
		}

		Console.Write("Фамилия: ");
		string lastName = Console.ReadLine()?.Trim();

		if (string.IsNullOrEmpty(lastName))
		{
			Console.WriteLine("Ошибка: Фамилия не может быть пустой.");
			return false;
		}

		Console.Write("Год рождения: ");
		if (!int.TryParse(Console.ReadLine(), out int birthYear) || birthYear < 1900 || birthYear > DateTime.Today.Year)
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
		try
		{
			_fileManager.SaveProfiles(AppInfo.Profiles);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Ошибка сохранения профиля: {ex.Message}");
		}

		Console.WriteLine($"Профиль создан! Добро пожаловать, {profile.GetInfo()}!");

		InitializeTodoList();
		CommandParser.Initialize(currentTodoList, AppInfo.CurrentProfile);
		return true;
	}

	private static void SaveAllData()
	{
		try
		{
			_fileManager.SaveProfiles(AppInfo.Profiles);
			foreach (var kvp in AppInfo.UserTodos)
			{
				_fileManager.SaveTodos(kvp.Key, kvp.Value);
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Ошибка сохранения данных: {ex.Message}");
		}
	}

}

   