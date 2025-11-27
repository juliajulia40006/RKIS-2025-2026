namespace TodoList.Commands;
public class DeleteCommand : ICommand
{
	public int TaskIndex { get; set; }
	public TodoList TodoList { get; set; }
	private TodoItem deletedItem;
	private int deletedIndex;

	public void Execute()
	{
		if (TaskIndex >= 1 && TaskIndex <= TodoList.Count)
		{
			int index = TaskIndex - 1;
			TodoItem item = TodoList[index];
			string deletedTask = item.Text;

			TodoList.Delete(index);
			Console.WriteLine($"Задача '{deletedTask}' удалена.");
		}
		else
		{
			Console.WriteLine("Неверный номер задачи!");
		}
	}

	public void Unexecute()
	{
		if (deletedItem != null)
		{
			AppInfo.Todos.Add(deletedItem);
		}
	}
}