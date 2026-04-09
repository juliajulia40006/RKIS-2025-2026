using TodoList.Exceptions;
using TodoList.Models;

namespace TodoList.Commands;

public class StatusCommand : ICommand
{
    public int TaskId { get; set; }
    public TodoStatus Status { get; set; }
	public TodoList TodoList { get; set; }

	private TodoStatus previousStatus;
	private int itemIndex;
	private TodoItem statusItem;
	public void Execute()
    {
		if (TodoList == null)
			throw new InvalidOperationException("Список задач не инициализирован.");

		if (TaskId < 1 || TaskId > TodoList.Count)
			throw new TaskNotFoundException(TaskId);

		int index = TaskId - 1;
		var item = TodoList[index];

		statusItem = item;
		previousStatus = item.Status;
		itemIndex = index;
		item.SetStatus(Status);
		TodoSynchronizer.SyncWithAppInfo(TodoList);
		string statusText = TodoItem.GetStatusText(Status);
		Console.WriteLine($"Задача '{statusItem.Text}' отмечена как '{statusText}'!");
	}

	public void Unexecute()
	{
		if (itemIndex >= 0 && itemIndex < TodoList.Count)
		{
			var item = TodoList[itemIndex];
			item.SetStatus(previousStatus);
			TodoSynchronizer.SyncWithAppInfo(TodoList);
		}
	}

	private string GetStatusText(TodoStatus status) => TodoItem.GetStatusText(status);
}