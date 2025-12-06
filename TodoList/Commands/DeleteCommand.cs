namespace TodoList.Commands;
public class DeleteCommand : ICommand
{
	public int TaskIndex { get; set; }
	public List<TodoItem> TodoItems { get; set; }
	private TodoItem deletedItem;
	private int deletedIndex;

	public void Execute()
	{
		if (TaskIndex >= 1 && TaskIndex <= TodoItems.Count)
		{
			deletedIndex = TaskIndex - 1;
			deletedItem = TodoItems[deletedIndex];
			string deletedTask = deletedItem.Text;
			TodoItems.RemoveAt(deletedIndex);
			Console.WriteLine($"Задача '{deletedTask}' удалена.");
		}
		else
		{
			Console.WriteLine("Неверный номер задачи!");
		}
	}

	public void Unexecute()
	{
		if (deletedItem != null && TodoItems != null)
		{
			if (deletedIndex >= 0 && deletedIndex <= TodoItems.Count)
			{
				TodoItems.Insert(deletedIndex, deletedItem);
			}
			else
			{
				TodoItems.Add(deletedItem);
			}
		}
	}
}