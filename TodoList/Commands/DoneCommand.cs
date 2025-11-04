using TodoList;
namespace TodoList.Commands;
public class DoneCommand : ICommand
{
    public int TaskIndex { get; set; }
    public TodoList TodoList { get; set; }

    public void Execute()
    {
        if (TaskIndex >= 1 && TaskIndex <= TodoList.Count)
        {
            int index = TaskIndex - 1;
            TodoItem item = TodoList.GetItem(index);
            item.MarkDone();
            Console.WriteLine($"Задача '{item.Text}' отмечена как выполненная!");
        }
        else
        {
            Console.WriteLine("Неверный номер задачи!");
        }
    }
}
