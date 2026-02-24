using TodoList.Exceptions;
namespace TodoList.Commands;
public class DeleteCommand : ICommand, IUndo
{
	public int TaskIndex { get; set; }
	public TodoList TodoList { get; set; }
	private TodoItem deletedItem;
	private int deletedIndex;

	public void Execute()
	{
		if (TodoList == null)
			throw new InvalidOperationException("Список задач не инициализирован.");

		if (TaskIndex < 1 || TaskIndex > TodoList.Count)
			throw new TaskNotFoundException(TaskIndex);

		deletedIndex = TaskIndex - 1;
		deletedItem = TodoList[deletedIndex];
		string deletedTask = deletedItem.Text;
		TodoList.Delete(deletedIndex);
		Console.WriteLine($"Задача '{deletedTask}' удалена.");
	}

	public void Unexecute()
	{
		if (deletedItem != null && TodoList != null)
		{
			var tempItems = TodoList.ToList();
			tempItems.Insert(deletedIndex, deletedItem);

			for (int i = TodoList.Count; i >= 0; i--)
			{
				TodoList.Delete(i);
			}

			foreach (var item in tempItems )
			{
				TodoList.Add(item);
			}
			FileManager.SyncTodoListWithAppInfo(TodoList);

			Console.WriteLine($"Задача '{deletedItem.Text}' восстановлена.");
		}
	}
}