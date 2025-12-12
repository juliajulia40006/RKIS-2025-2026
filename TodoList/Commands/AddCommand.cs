namespace TodoList.Commands;
public class AddCommand : ICommand
{
    public bool Multiline { get; set; } = false;
    public string TaskText { get; set; } = "";
	public List<TodoItem> TodoItems { get; set; }

	private TodoItem _addedItem;

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
			Console.WriteLine($"Добавлено: {TaskText}.");
        }
    }

	public void Unexecute()
	{
		if (_addedItem != null && TodoItems != null && TodoItems.Count > 0)
		{
			int index = TodoItems.Count - 1;
			if (index >= 0 && TodoItems[index] == _addedItem)
			{
				TodoItems.RemoveAt(index);
			}
			else
			{
				for (int i = 0; i < TodoItems.Count; i++)
				{
					if (TodoItems[i] == _addedItem)
					{
						TodoItems.RemoveAt(i);
						break;
					}
				}
			}
		}
	}
}
