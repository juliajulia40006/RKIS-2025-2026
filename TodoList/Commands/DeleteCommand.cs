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
			deletedIndex = TaskIndex - 1;
			deletedItem = TodoList[deletedIndex];
			string deletedTask = deletedItem.Text;
			TodoList.Delete(deletedIndex);
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
			TodoList.Add(deletedItem);
		}
	}
}