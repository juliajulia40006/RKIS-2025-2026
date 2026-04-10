using TodoList;
using TodoList.Commands;
using TodoList.Data;
using TodoList.Exceptions;
using TodoList.Models;
using TodoList.Services;

class Program
{
	private static TodoList.TodoList currentTodoList;
	public static TodoList.TodoList CurrentTodoList => currentTodoList;
	private static ProfileRepository _profileRepository;
	private static TodoRepository _todoRepository;
static void Main(string[] args)
	{
		try
		{
			Console.WriteLine("The program was made by Deinega and Piyagova");

			using (var db = new AppDbContext())
			{
				db.Database.EnsureCreated();
			}

			_profileRepository = new ProfileRepository();
			_todoRepository = new TodoRepository();

			try
			{
				AppInfo.Profiles = _profileRepository.GetAll();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка загрузки профилей: {ex.Message}");
				AppInfo.Profiles = new List<Profile>();
			}

			string dataDirectory = "data";
			var fileManager = new FileManager(dataDirectory);
			TodoSynchronizer.Initialize(fileManager);
			if (!Directory.Exists(dataDirectory))
			{
				Directory.CreateDirectory(dataDirectory);
			}
			using (var db = new AppDbContext())
			{
				db.Database.EnsureCreated();
			}

			_profileRepository = new ProfileRepository();
			_todoRepository = new TodoRepository();

			try
			{
				AppInfo.Profiles = _profileRepository.GetAll();
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
		foreach (var todo in userTodos)
		{
			if (todo.ProfileId == Guid.Empty)
			{
				todo.ProfileId = AppInfo.CurrentProfileId.Value;
			}
		}

		currentTodoList = new TodoList.TodoList();
		foreach (var todo in userTodos)
		{
			currentTodoList.Add(todo);
		}
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
			using var context = new AppDbContext();

			string loginLower = login.ToLower();
			var profile = context.Profiles
				.FirstOrDefault(p => p.Login.ToLower() == loginLower && p.Password == password);

			if (profile == null)
				throw new ProfileNotFoundException(login);

			if (!AppInfo.Profiles.Any(p => p.Id == profile.Id))
			{
				AppInfo.Profiles.Add(profile);
			}

			AppInfo.CurrentProfileId = profile.Id;
			AppInfo.CurrentProfile = profile;

			try
			{
				var todos = context.Todos
					.Where(t => t.ProfileId == profile.Id)
					.ToList();
				AppInfo.UserTodos[profile.Id] = todos;
			}

			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка загрузки задач: {ex.Message}");
				AppInfo.UserTodos[profile.Id] = new List<TodoItem>();
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

		using (var checkContext = new AppDbContext())
		{
			string loginLower = login.ToLower();
			var existingProfile = checkContext.Profiles
				.FirstOrDefault(p => p.Login.ToLower() == loginLower);

			if (existingProfile != null)
			{
				Console.WriteLine("Этот логин уже занят.");
				return false;
			}
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

		try
		{
			using var context = new AppDbContext();
			context.Profiles.Add(profile);
			context.SaveChanges();
			Console.WriteLine($"Профиль сохранен в БД с ID: {id}");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Ошибка сохранения профиля в БД: {ex.Message}");
			return false;
		}

		AppInfo.Profiles.Add(profile);
		AppInfo.CurrentProfileId = id;
		AppInfo.CurrentProfile = profile;
		AppInfo.UserTodos[id] = new List<TodoItem>();

		Console.WriteLine($"Профиль создан! Добро пожаловать, {profile.GetInfo()}!");

		InitializeTodoList();
		CommandParser.Initialize(currentTodoList, AppInfo.CurrentProfile);
		return true;
	}

private static void SaveAllData()
{
    try
    {
        foreach (var profile in AppInfo.Profiles)
        {
            try
            {
                using var context = new AppDbContext();
                
                var existingProfile = context.Profiles.Find(profile.Id);
                
                if (existingProfile == null)
                {
                    context.Profiles.Add(profile);
                    context.SaveChanges();
                    Console.WriteLine($"Профиль {profile.Login} (ID: {profile.Id}) добавлен в БД");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения профиля {profile.Login}: {ex.Message}");
            }
        }

        foreach (var kvp in AppInfo.UserTodos)
        {
            var profileId = kvp.Key;
            var todos = kvp.Value;

            if (todos.Count == 0) continue;

            Console.WriteLine($"Сохранение {todos.Count} задач для профиля ID: {profileId}");

            try
            {
                using var context = new AppDbContext();
                
                var profile = context.Profiles.Find(profileId);
                if (profile == null)
                {
                    Console.WriteLine($"ОШИБКА: Профиль с ID {profileId} не найден в БД!");
                    continue;
                }

                var existingTodos = context.Todos
                    .Where(t => t.ProfileId == profileId)
                    .ToList();

                foreach (var existingTodo in existingTodos)
                {
                    if (!todos.Any(t => t.Id == existingTodo.Id))
                    {
                        context.Todos.Remove(existingTodo);
                    }
                }
                
                context.SaveChanges();

                foreach (var todo in todos)
                {
                    var todoForDb = new TodoItem(todo.Text)
                    {
                        ProfileId = profileId
                    };
                    todoForDb.SetStatus(todo.Status);
                    todoForDb.SetLastUpdate(todo.LastUpdate);
                    
                    if (todo.Id > 0)
                    {
                        var existingTodo = context.Todos.Find(todo.Id);
                        if (existingTodo != null)
                        {
                            existingTodo.UpdateText(todo.Text);
                            existingTodo.SetStatus(todo.Status);
                            existingTodo.SetLastUpdate(todo.LastUpdate);
                        }
                        else
                        {
                            context.Todos.Add(todoForDb);
                        }
                    }
                    else
                    {
                        context.Todos.Add(todoForDb);
                    }
                }
                
                context.SaveChanges();
                Console.WriteLine($"Задачи для профиля {profileId} успешно сохранены");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении задач: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Внутренняя ошибка: {ex.InnerException.Message}");

                    foreach (var todo in todos)
                    {
                        try
                        {
                            using var singleContext = new AppDbContext();
                            var todoForDb = new TodoItem(todo.Text)
                            {
                                ProfileId = profileId
                            };
                            todoForDb.SetStatus(todo.Status);
                            todoForDb.SetLastUpdate(todo.LastUpdate);
                            
                            singleContext.Todos.Add(todoForDb);
                            singleContext.SaveChanges();
                            Console.WriteLine($"  Задача '{todo.Text}' сохранена успешно");
                        }
                        catch (Exception singleEx)
                        {
                            Console.WriteLine($"  Ошибка сохранения задачи '{todo.Text}': {singleEx.Message}");
                            if (singleEx.InnerException != null)
                            {
                                Console.WriteLine($"  Внутренняя ошибка: {singleEx.InnerException.Message}");
                            }
                        }
                    }
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Критическая ошибка: {ex.Message}");
    }
}
	private static void ImportFromCsvToDb()
	{
		string dataDirectory = "data";
		var fileManager = new FileManager(dataDirectory);

		if (File.Exists(Path.Combine(dataDirectory, "profile.csv")))
		{
			var csvProfiles = fileManager.LoadProfiles().ToList();
			foreach (var profile in csvProfiles)
			{
				var existing = _profileRepository.GetById(profile.Id);
				if (existing == null)
				{
					_profileRepository.Add(profile);
					Console.WriteLine($"Импортирован профиль: {profile.Login}");
				}
			}
			AppInfo.Profiles = _profileRepository.GetAll().ToList();
		}
	}

}

   