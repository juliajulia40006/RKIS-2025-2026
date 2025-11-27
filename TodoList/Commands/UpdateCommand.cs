namespace TodoList.Commands;
public class UpdateCommand : ICommand
{
    public int TaskIndex { get; set; }
    public string NewText { get; set; } = "";
    public TodoList TodoList { get; set; }
	private int itemIndex;
	private string oldText;

	public void Execute()
    {
        if (TaskIndex >= 1 && TaskIndex <= TodoList.Count)
        {
            int index = TaskIndex - 1;
            TodoItem item = TodoList[index];
            oldText = item.Text;
			itemIndex = index;

			if (string.IsNullOrEmpty(NewText))
            {
                Console.WriteLine("Ошибка: Новый текст задачи не может быть пустым");
                return;
            }

            item.UpdateText(NewText);
            Console.WriteLine($"Задача обновлена: '{oldText}' -> '{NewText}'");
        }
        else
        {
            Console.WriteLine("Ошибка: Используйте: update \"номер\" новый текст");
        }
    }

	public void Unexecute()
	{
		if (itemIndex >= 0 && itemIndex < TodoList.Count && oldText != null)
		{
			TodoItem item = TodoList[itemIndex];
			item.UpdateText(oldText);
		}
	}
}