namespace TodoList.Commands;
public class AddCommand : ICommand
{
    public bool Multiline { get; set; } = false;
    public string TaskText { get; set; } = "";
    public TodoList TodoList { get; set; }
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
			TodoList.Add(_addedItem);
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
			TodoList.Add(_addedItem);
			Console.WriteLine($"Добавлено: {TaskText}.");
        }
    }

	public void Unexecute()
	{
		if (_addedItem != null && TodoList.Count > 0)
		{
			int index = TodoList.Count - 1;
			if (index >= 0 && TodoList[index] == _addedItem)
			{
				TodoList.Delete(index);
			}
			else
			{
				for (int i = 0; i < TodoList.Count; i++)
				{
					if (TodoList[i] == _addedItem)
					{
						TodoList.Delete(i);
						break;
					}
				}
			}
		}
	}
}