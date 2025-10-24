public class TodoList
{
    private TodoItem[] items;
    private int count;

    public int Count => count;

    public TodoList()
    {
        items = new TodoItem[2];
        count = 0;
    }

    public void Add(TodoItem item)
    {
        if (count >= items.Length)
        {
            IncreaseArray();
        }

        items[count] = item;
        count++;
    }

    public void Delete(int index)
    {
        if (index < 0 || index >= count)
            throw new ArgumentOutOfRangeException(nameof(index));

        for (int i = index; i < count - 1; i++)
        {
            items[i] = items[i + 1];
        }

        count--;
        items[count] = null;
    }

    public TodoItem GetItem(int index)
    {
        if (index < 0 || index >= count)
            throw new ArgumentOutOfRangeException(nameof(index));

        return items[index];
    }

    public void View(bool showIndex, bool showStatus, bool showDate)
    {
        if (count == 0)
        {
            Console.WriteLine("\nЗадач нет");
            return;
        }

        Console.WriteLine("\nВаши задачи:");

        int indexWidth = 6;
        int taskWidth = 30;
        int statusWidth = 12;
        int dateWidth = 19;

        string header = "";
        if (showIndex) header += "Индекс".PadRight(indexWidth) + " ";
        header += "Задача".PadRight(taskWidth) + " ";
        if (showStatus) header += "Статус".PadRight(statusWidth) + " ";
        if (showDate) header += "Дата изменения".PadRight(dateWidth);

        Console.WriteLine(header);
        Console.WriteLine(new string('-', header.Length));

        for (int i = 0; i < count; i++)
        {
            string line = "";
            var item = items[i];

            if (showIndex)
            {
                line += (i + 1).ToString().PadRight(indexWidth) + " ";
            }

            string taskText = GetFirstLine(item.Text);
            string shortTask = taskText.Length > 30 ? taskText.Substring(0, 27) + "..." : taskText;
            line += shortTask.PadRight(taskWidth) + " ";

            if (showStatus)
            {
                string status = item.IsDone ? "выполнено" : "не выполнено";
                line += status.PadRight(statusWidth) + " ";
            }

            if (showDate)
            {
                line += item.LastUpdate.ToString("dd.MM.yyyy HH:mm").PadRight(dateWidth);
            }

            Console.WriteLine(line.TrimEnd());
        }
    }

    private void IncreaseArray()
    {
        TodoItem[] newItems = new TodoItem[items.Length * 2];
        for (int i = 0; i < items.Length; i++)
        {
            newItems[i] = items[i];
        }
        items = newItems;

        Console.WriteLine($"Массив расширен до {items.Length} элементов");
    }

    private string GetFirstLine(string task)
    {
        if (string.IsNullOrEmpty(task))
            return task;

        int newLineIndex = task.IndexOf('\n');
        if (newLineIndex >= 0)
        {
            return task.Substring(0, newLineIndex) + "...";
        }

        return task;
    }
}