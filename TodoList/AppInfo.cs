	using TodoList.Commands;

namespace TodoList;
class AppInfo
{
	public static List<Profile> Profiles { get; set; } = new List<Profile>();
	public static Guid? CurrentProfileId { get; set; }
	public static Dictionary<Guid, TodoList> UserTodos { get; set; } = new Dictionary<Guid, TodoList>();
	public static TodoList Todos { get; set; }
	public static Profile CurrentProfile { get; set; }
	public static Stack<ICommand> undoStack = new Stack<ICommand>();
	public static Stack<ICommand> redoStack = new Stack<ICommand>();
	public static TodoList GetCurrentTodos()
	{
		if (CurrentProfileId.HasValue && UserTodos.ContainsKey(CurrentProfileId.Value))
		{
			return UserTodos[CurrentProfileId.Value];
		}
		return new TodoList();
	}
	public static void SaveUserTodos(Guid userId)
	{
		if (UserTodos.ContainsKey(userId))
		{
			string filePath = Path.Combine("data", $"todos_{userId}.csv");
			FileManager.SaveTodos(UserTodos[userId], filePath);
		}
	}
	public static TodoList LoadUserTodos(Guid userId)
	{
		string filePath = Path.Combine("data", $"todos_{userId}.csv");
		if (File.Exists(filePath))
		{
			return FileManager.LoadTodos(filePath);
		}
		return new TodoList();
	}

}
