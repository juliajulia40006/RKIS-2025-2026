using static TodoList.TodoItem;

namespace TodoList.Commands;

public class StatusCommand : ICommand
{
    public int TaskIndex { get; set; }
    public TodoStatus Status { get; set; }
    public TodoList TodoList { get; set; }
	private TodoStatus previousStatus;
	private int itemIndex;
	public void Execute()
    {
        if (TaskIndex >= 1 && TaskIndex <= TodoList.Count)
        {
            int index = TaskIndex - 1;
            var item = TodoList[index];
			previousStatus = item.Status;
            item.SetStatus(Status);
            string statusText = GetStatusText(Status);
            Console.WriteLine($"Задача '{item.Text}' отмечена как '{statusText}'!");
        }
        else
        {
            Console.WriteLine("Неверный номер задачи!");
        }
    }

	public void Unexecute()
	{
		if (itemIndex >= 0 && itemIndex < AppInfo.Todos.Count)
		{
			var item = AppInfo.Todos[itemIndex];
			item.SetStatus(previousStatus);
		}
	}

	private string GetStatusText(TodoStatus status)
    {
        return TodoItem.GetStatusText(status);
    }
}