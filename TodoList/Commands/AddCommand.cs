namespace TodoList.Commands;
public class AddCommand : ICommand, IUndo
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
			if (TodoList != null)
			{
				int index = TodoList.Count - 1;
			}
			TodoSynchronizer.SyncWithAppInfo(TodoList);
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
			TodoSynchronizer.SyncWithAppInfo(TodoList);
			Console.WriteLine($"Добавлено: {TaskText}.");
        }	
    }

	public void Unexecute()
	{
		if (_addedItem != null && TodoList != null && TodoList.Count > 0)
		{
			int index = - 1;
			for (int i = 0; i < TodoList.Count; i++)
			{
				if (TodoList[i] == _addedItem)
				{
					index = i;
					break;
				}
			}

			if (index >= 0)
			{
				TodoList.Delete(index);
				TodoSynchronizer.SyncWithAppInfo(TodoList);
			}
		}
	}
}
