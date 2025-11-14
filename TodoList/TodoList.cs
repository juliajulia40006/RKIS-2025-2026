namespace TodoList;
public class TodoList
{
    private List<TodoItem> items;
    public int Count => items.Count;

    public TodoList()
    {
        items = new List<TodoItem>();
        count = 0;
    }

    public void Add(TodoItem item)
    {
        items.Add(item);
    }

    public void Delete(int index)
    {
        if (index < 0 || index >= count)
            throw new ArgumentOutOfRangeException(nameof(index));


        items.RemoveAt(index);
    }

    public TodoItem GetItem(int index)
    {
        if (index < 0 || index >= count)
            throw new ArgumentOutOfRangeException(nameof(index));

        return items[index];
    }

    public TodoItem this[int index]
    {
        get
        {
            if (index < 0 || index >= items.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            return items[index];
        }
    }
    public IEnumerator<TodoItem> GetEnumerator()
    {
        foreach (var item in items)
        {
            yield return item;
        }
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    public void View(bool showIndex, bool showStatus, bool showDate)
    {
        if (count == 0)
        {
            Console.WriteLine("\nЗадач нет");
            return;
        }

        int indexWidth = 6;
        int taskWidth = 30;
        int statusWidth = 12;
        int dateWidth = 19;

        string header = "";
        if (showIndex) header += "Индекс".PadRight(indexWidth) + " ";
        header += "Задачи".PadRight(taskWidth) + " ";
        if (showStatus) header += "Статус".PadRight(statusWidth) + " ";
        if (showDate) header += "Дата изменения".PadRight(dateWidth);

        Console.WriteLine(header);
        Console.WriteLine(new string('-', header.Length));

        for (int i = 0; i < items.Count; i++)
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
                string status = item.Status ? "выполнено" : "не выполнено";
                line += status.PadRight(statusWidth) + " ";
            }

            if (showDate)
            {
                line += item.LastUpdate.ToString("dd.MM.yyyy HH:mm").PadRight(dateWidth);
            }

            Console.WriteLine(line.TrimEnd());
        }
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