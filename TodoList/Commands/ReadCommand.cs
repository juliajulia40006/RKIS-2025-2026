
namespace TodoList.Commands;
public class ReadCommand : ICommand
{
    public int TaskIndex { get; set; }
	public List<TodoItem> TodoItems { get; set; }

	public void Execute()
    {
		if (TodoItems != null && TaskIndex >= 1 && TaskIndex <= TodoItems.Count)
		{
            int index = TaskIndex - 1;
            TodoItem item = TodoItems[index];
            Console.WriteLine($"Задача #{TaskIndex}:");
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