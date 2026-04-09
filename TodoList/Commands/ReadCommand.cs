using TodoList.Models;
namespace TodoList.Commands;
public class ReadCommand : ICommand
{
    public int TaskId { get; set; }
	public TodoList TodoList { get; set; }

	public void Execute()
    {
		if (TodoList != null && TaskId >= 1 && TaskId <= TodoList.Count)
		{
            int index = TaskId - 1;
            TodoItem item = TodoList[index];
            Console.WriteLine($"Задача #{TaskId}:");
            Console.WriteLine(item.GetFullInfo());
        }
        else
        {
            Console.WriteLine("Неверный номер задачи!");
        }
    }
	public void Unexecute()
	{

	}
}