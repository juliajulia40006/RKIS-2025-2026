using static TodoList.TodoList;
namespace TodoList.Commands;

public class StatusCommand : ICommand
{
    public int TaskIndex { get; set; }
    public TodoStatus Status { get; set; }
	public TodoList TodoList { get; set; }

	private TodoStatus previousStatus;
	private int itemIndex;
	private TodoItem statusItem;
	public void Execute()
    {
        if (TaskIndex >= 1 && TaskIndex <= TodoList.Count)
        {
            int index = TaskIndex - 1;
			var item = TodoList[index];

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
		if (itemIndex >= 0 && itemIndex < TodoList.Count)
		{
			var item = TodoList[itemIndex];
			item.SetStatus(previousStatus);
		}
	}

	private string GetStatusText(TodoStatus status)
    {
        return TodoItem.GetStatusText(status);
    }
}