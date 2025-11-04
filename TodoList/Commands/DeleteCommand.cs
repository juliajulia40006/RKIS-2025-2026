using TodoList;
namespace TodoList.Commands;
public class DeleteCommand : ICommand
{
    public int TaskIndex { get; set; }
    public TodoList TodoList { get; set; }

    public void Execute()
    {
        if (TaskIndex >= 1 && TaskIndex <= TodoList.Count)
        {
            int index = TaskIndex - 1;
            TodoItem item = TodoList.GetItem(index);
            string deletedTask = item.Text;

            TodoList.Delete(index);
            Console.WriteLine($"Задача '{deletedTask}' удалена.");
        }
        else
        {
            Console.WriteLine("Неверный номер задачи!");
        }
    }
}