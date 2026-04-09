using TodoList.Exceptions;
using TodoList.Models;
namespace TodoList.Commands;
public class DeleteCommand : ICommand, IUndo
{
	public int TaskId { get; set; }
	public TodoList TodoList { get; set; }
	private TodoItem deletedItem;
	private int deletedIndex;

	public void Execute()
	{
		if (TodoList == null)
			throw new InvalidOperationException("Список задач не инициализирован.");

		if (TaskId < 1 || TaskId > TodoList.Count)
			throw new TaskNotFoundException(TaskId);

		deletedItem = TodoList.FirstOrDefault(t => t.Id == TaskId);
		if (deletedItem == null) throw new TaskNotFoundException(TaskId);

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
			TodoSynchronizer.SyncWithAppInfo(TodoList);
			Console.WriteLine($"Задача '{deletedItem.Text}' восстановлена.");
		}
	}
}