namespace TodoList.Commands;
public class UpdateCommand : ICommand
{
    public int TaskIndex { get; set; }
    public string NewText { get; set; } = "";
	public List<TodoItem> TodoItems { get; set; }

	private int itemIndex;
	private string oldText;
	private TodoItem updatedItem;
	private TodoList todoList;

	public void Execute()
    {
		if (TodoItems != null && TaskIndex >= 1 && TaskIndex <= TodoItems.Count)
		{
            int index = TaskIndex - 1;
			updatedItem = TodoItems[index];
			oldText = updatedItem.Text;
			itemIndex = index;

			if (string.IsNullOrEmpty(NewText))
            {
                Console.WriteLine("Ошибка: Новый текст задачи не может быть пустым");
                return;
            }

			if (todoList == null)
			{
				todoList = new TodoList();
			}

			updatedItem.UpdateText(NewText);
            Console.WriteLine($"Задача обновлена: '{oldText}' -> '{NewText}'");
        }
        else
        {
            Console.WriteLine("Ошибка: Используйте: update \"номер\" новый текст");
        }
    }

	public void Unexecute()
	{
		if (updatedItem != null && oldText != null)
		{
			updatedItem.UpdateText(oldText);
		}
	}
}