using TodoList.Commands;

namespace TodoList;
class AppInfo
{
	public static TodoList Todos { get; set; }
	public static Profile CurrentProfile { get; set; }
	public static Stack<ICommand> undoStack = new Stack<ICommand>();
	public static Stack<ICommand> redoStack = new Stack<ICommand>();
}
