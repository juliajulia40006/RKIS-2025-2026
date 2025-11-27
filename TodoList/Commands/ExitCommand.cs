
namespace TodoList.Commands;
public class ExitCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Выход...");
        return;
    }
	public void Unexecute()
	{

	}
}
