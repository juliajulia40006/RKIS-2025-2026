using TodoList;
namespace TodoList.Commands;

public class ProfileCommand : ICommand
{
    public Profile Profile { get; set; }

    public void Execute()
    {
        Console.WriteLine($"\n{Profile.GetInfo()}");
    }
}