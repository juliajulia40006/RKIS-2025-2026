using TodoList.Exceptions;
using TodoList.Models;
namespace TodoList.Commands;
public class UpdateCommand : ICommand, IUndo
{
    public int TaskId { get; set; }
    public string NewText { get; set; } = "";
	public TodoList TodoList { get; set; }

	private int itemIndex;
	private string oldText;
	private TodoItem updatedItem;

	public void Execute()
    {
		if (TodoList == null)
			throw new InvalidOperationException("Список задач не инициализирован.");

		if (TaskId < 1 || TaskId > TodoList.Count)
			throw new TaskNotFoundException(TaskId);

		if (string.IsNullOrEmpty(NewText))
			throw new InvalidArgumentException("Новый текст задачи не может быть пустым.");

		int index = TaskId - 1;
		updatedItem = TodoList[index];
		oldText = updatedItem.Text;
		itemIndex = index;

		TodoList.Update(index, NewText);
		TodoSynchronizer.SyncWithAppInfo(TodoList);
		Console.WriteLine($"Задача обновлена: '{oldText}' -> '{NewText}'");
	}

	public void Unexecute()
	{
		if (updatedItem != null && oldText != null && TodoList != null)
		{
			TodoList.Update(itemIndex, oldText);
			TodoSynchronizer.SyncWithAppInfo(TodoList);
		}
	}
}