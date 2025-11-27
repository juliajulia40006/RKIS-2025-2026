
namespace TodoList.Commands;
public class ReadCommand : ICommand
{
    public int TaskIndex { get; set; }
    public TodoList TodoList { get; set; }

    public void Execute()
    {
        if (TaskIndex >= 1 && TaskIndex <= TodoList.Count)
        {
            int index = TaskIndex - 1;
            TodoItem item = TodoList[index];
            Console.WriteLine($"Задача #{TaskIndex}:");
            Console.WriteLine(item.GetFullInfo());
        }
        else
        {
            Console.WriteLine("Неверный номер задачи!");
        }
    }
}