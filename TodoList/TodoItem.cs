namespace TodoList;
public class TodoItem
{
    public enum TodoStatus
    {
        NotStarted,
        InProgress,
        Completed,
        Postponed,
        Failed
    }

    public string Text { get; private set; }
    public TodoStatus Status { get; private set; }
    public DateTime LastUpdate { get; private set; }

    public TodoItem(string text)
    {
        Text = text;
        Status = TodoStatus.NotStarted;
        LastUpdate = DateTime.Now;
    }

    public void MarkDone()
    {
        Status = TodoStatus.Completed;
        LastUpdate = DateTime.Now;
    }
    public void SetStatus(TodoStatus status)
    {
        Status = status;
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
        string status = GetStatusText(Status);
        return $"{shortText} | {status} | {LastUpdate:dd.MM.yyyy HH:mm}";
    }

    public string GetFullInfo()
    {
        string status = GetStatusText(Status);
        return $"Текст: \n{Text}\nСтатус: {status}\nДата изменения: {LastUpdate}";
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
    private string GetStatusText(TodoStatus status)
    {
        return status switch
        {
            TodoStatus.NotStarted => "не начато",
            TodoStatus.InProgress => "в процессе",
            TodoStatus.Completed => "выполнено",
            TodoStatus.Postponed => "отложено",
            TodoStatus.Failed => "провалено"
        };
    }
}