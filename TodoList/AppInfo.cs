using TodoList.Commands;


namespace TodoList;
class AppInfo
{
	public static List<Profile> Profiles { get; set; } = new List<Profile>();
	public static Guid? CurrentProfileId { get; set; }
	public static Dictionary<Guid, List<TodoItem>> UserTodos { get; set; } = new Dictionary<Guid, List<TodoItem>>();
	public static TodoList Todos { get; set; }
	public static Profile CurrentProfile { get; set; }
	public static Stack<ICommand> undoStack = new Stack<ICommand>();
	public static Stack<ICommand> redoStack = new Stack<ICommand>();
	public static List<TodoItem> GetCurrentTodos()
	{
		if (CurrentProfileId.HasValue && UserTodos.ContainsKey(CurrentProfileId.Value))
		{
			return UserTodos[CurrentProfileId.Value];
		}
		return new List<TodoItem>();
	}

	public static void SaveUserTodos(Guid userId)
	{
		if (UserTodos.ContainsKey(userId))
		{
			string filePath = Path.Combine("data", $"todos_{userId}.csv");
			FileManager.SaveUserTodos(userId, UserTodos[userId], filePath);
		}
	}
	public static List<TodoItem> LoadUserTodos(Guid userId)
	{
		string filePath = Path.Combine("data", $"todos_{userId}.csv");
		if (File.Exists(filePath))
		{
			return FileManager.LoadUserTodos(filePath);
		}
		return new List<TodoItem>();
	}

}
