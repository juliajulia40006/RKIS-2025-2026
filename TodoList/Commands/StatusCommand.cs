using static TodoList.TodoItem;

namespace TodoList.Commands;

public class StatusCommand : ICommand
{
    public int TaskIndex { get; set; }
    public TodoStatus Status { get; set; }
	public List<TodoItem> TodoItems { get; set; }

	private TodoStatus previousStatus;
	private int itemIndex;
	private TodoItem statusItem;
	public void Execute()
    {
        if (TaskIndex >= 1 && TaskIndex <= TodoItems.Count)
        {
            int index = TaskIndex - 1;
			var item = TodoItems[index];

			statusItem = item;
			previousStatus = item.Status;
			itemIndex = index;
			item.SetStatus(Status);
            string statusText = TodoItem.GetStatusText(Status);
            Console.WriteLine($"Задача '{statusItem.Text}' отмечена как '{statusText}'!");
        }
        else
        {
            Console.WriteLine("Неверный номер задачи!");
        }
    }

	public void Unexecute()
	{
		if (itemIndex >= 0 && itemIndex < TodoItems.Count)
		{
			var item = TodoItems[itemIndex];
			item.SetStatus(previousStatus);
		}
	}

	private string GetStatusText(TodoStatus status)
    {
        return TodoItem.GetStatusText(status);
    }
}