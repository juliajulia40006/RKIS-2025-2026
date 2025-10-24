public class TodoItem
{
    public string Text { get; private set; }
    public bool IsDone { get; private set; }
    public DateTime LastUpdate { get; private set; }

    public TodoItem(string text)
    {
        Text = text;
        IsDone = false;
        LastUpdate = DateTime.Now;
    }

    public void MarkDone()
    {
        IsDone = true;
        LastUpdate = DateTime.Now;
    }

    public void UpdateText(string newText)
    {
        Text = newText;
        LastUpdate = DateTime.Now;
    }

    public string GetShortInfo()
    {
        string shortText = GetFirstLine(Text);
        shortText = shortText.Length > 30 ? shortText.Substring(0, 27) + "..." : shortText;
        string status = IsDone ? "выполнено" : "не выполнено";
        return $"{shortText} | {status} | {LastUpdate:dd.MM.yyyy HH:mm}";
    }

    public string GetFullInfo()
    {
        string status = IsDone ? "выполнено" : "не выполнено";
        return $"Текст: {Text}\nСтатус: {status}\nДата изменения: {LastUpdate}";
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