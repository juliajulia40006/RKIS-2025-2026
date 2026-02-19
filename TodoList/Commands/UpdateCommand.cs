namespace TodoList.Commands;
public class UpdateCommand : ICommand
{
    public int TaskIndex { get; set; }
    public string NewText { get; set; } = "";
	public TodoList TodoList { get; set; }

	private int itemIndex;
	private string oldText;
	private TodoItem updatedItem;

	public void Execute()
    {
		if (TodoList != null && TaskIndex >= 1 && TaskIndex <= TodoList	.Count)
		{
            int index = TaskIndex - 1;
			updatedItem = TodoList[index];
			oldText = updatedItem.Text;
			itemIndex = index;

			if (string.IsNullOrEmpty(NewText))
            {
                Console.WriteLine("Ошибка: Новый текст задачи не может быть пустым");
                return;
            }

			TodoList.Update(TaskIndex, NewText);
			Console.WriteLine($"Задача обновлена: '{oldText}' -> '{NewText}'");
		}

		else
        {
            Console.WriteLine("Ошибка: Используйте: update \"номер\" новый текст");
        }
    }

	public void Unexecute()
	{
		if (updatedItem != null && oldText != null && TodoList != null)
		{
			TodoList.Update(itemIndex + 1, oldText);
		}
	}
}