using static TodoList.TodoItem;

namespace TodoList.Commands;

public class StatusCommand : ICommand
{
    public int TaskIndex { get; set; }
    public TodoStatus Status { get; set; }
    public TodoList TodoList { get; set; }

    public void Execute()
    {
        if (TaskIndex >= 1 && TaskIndex <= TodoList.Count)
        {
            int index = TaskIndex - 1;
            TodoItem item = TodoList.GetItem(index);
            item.SetStatus(Status);
            string statusText = GetStatusText(Status);
            Console.WriteLine($"Задача '{item.Text}' отмечена как '{statusText}'!");
        }
        else
        {
            Console.WriteLine("Неверный номер задачи!");
        }
    }

    private string GetStatusText(TodoStatus status)
    {
        return TodoItem.GetStatusText(status);
    }
}