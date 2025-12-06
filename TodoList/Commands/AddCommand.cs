namespace TodoList.Commands;
public class AddCommand : ICommand
{
    public bool Multiline { get; set; } = false;
    public string TaskText { get; set; } = "";
	public List<TodoItem> TodoItems { get; set; }

	private TodoItem _addedItem;
	private int _addedItemIndex = -1;
	public void Execute()
    {
        if (Multiline)
        {
            Console.WriteLine("Многострочный режим. Вводите задачи (для завершения введите !end):");
            string multilineTask = "";
            string line;

            while (true)
            {
                Console.Write("> ");
                line = Console.ReadLine();

                if (line == "!end")
                {
                    break;
                }

                if (!string.IsNullOrEmpty(multilineTask))
                {
                    multilineTask += "\n";
                }
                multilineTask += line;
            }

            if (string.IsNullOrEmpty(multilineTask))
            {
                Console.WriteLine("Ошибка: Текст задачи не может быть пустым.");
                return;
            }

			_addedItem = new TodoItem(multilineTask);
			TodoItems.Add(_addedItem);
			_addedItemIndex = TodoItems?.IndexOf(_addedItem) ?? -1;
			Console.WriteLine("Добавлена многострочная задача");
        }
        else
        {
            if (string.IsNullOrEmpty(TaskText))
            {
                Console.WriteLine("Ошибка: Текст задачи не может быть пустым.");
                return;
            }

			_addedItem = new TodoItem(TaskText);
			TodoItems.Add(_addedItem);
			_addedItemIndex = TodoItems?.IndexOf(_addedItem) ?? -1;
			Console.WriteLine($"Добавлено: {TaskText}.");
        }
    }

	public void Unexecute()
	{
		if (_addedItem != null && TodoItems != null && TodoItems.Count > 0)
		{
			if (_addedItemIndex >= 0 && _addedItemIndex < TodoItems.Count)
			{
				if (TodoItems[_addedItemIndex] == _addedItem ||
					TodoItems[_addedItemIndex].Text == _addedItem.Text)
				{
					TodoItems.RemoveAt(_addedItemIndex);
					return;
				}
			}

			for (int i = TodoItems.Count - 1; i >= 0; i--)
			{
				if (TodoItems[i] == _addedItem ||
					TodoItems[i].Text == _addedItem.Text)
				{
					TodoItems.RemoveAt(i);
					break;
				}
			}
		}
	}
}
