
namespace TodoList.Commands;
public class ViewCommand : ICommand
{
    public bool ShowIndex { get; set; } = false;
    public bool ShowStatus { get; set; } = false;
    public bool ShowDate { get; set; } = false;
    public bool ShowAll { get; set; } = false;
	public List<TodoItem> TodoItems { get; set; }

	public void Execute()
    {
        if (ShowAll)
        {
            ShowIndex = true;
            ShowStatus = true;
            ShowDate = true;
        }

        TodoList.View(TodoItems, ShowIndex, ShowStatus, ShowDate);
    }
	public void Unexecute()
	{

	}
}