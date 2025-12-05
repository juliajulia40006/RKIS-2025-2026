	using TodoList.Commands;

namespace TodoList;
class AppInfo
{
	public static TodoList Todos { get; set; }
	public static List<Profile> Profiles { get; set; } = new List<Profile>();
	public static Guid? CurrentProfileId { get; set; }
	public static Dictionary<Guid, List<TodoItem>> UserTodos { get; set; } = new Dictionary<Guid, List<TodoItem>>();

	public static Stack<ICommand> undoStack = new Stack<ICommand>();
	public static Stack<ICommand> redoStack = new Stack<ICommand>();
	public static List<TodoItem> GetCurrentTodos()
	{
		if (CurrentProfileId.HasValue && UserTodos.ContainsKey(CurrentProfileId.Value))
			return UserTodos[CurrentProfileId.Value];
		return new List<TodoItem>();
	}

}
