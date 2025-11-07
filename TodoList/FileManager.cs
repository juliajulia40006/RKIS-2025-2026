namespace TodoList;

public static class FileManager
{
    public static void EnsureDataDirectory(string dirPath)
    {
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
    }
}

public static void SaveProfile(Profile profile, string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(profile.FirstName);
            writer.WriteLine(profile.LastName);
            writer.WriteLine(profile.BirthYear);
        }
    }

    public static Profile LoadProfile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return null;
        }

        using (StreamReader reader = new StreamReader(filePath))
        {
            string firstName = reader.ReadLine();
            string lastName = reader.ReadLine();
            int birthYear = int.Parse(reader.ReadLine());

            return new Profile(firstName, lastName, birthYear);
        }
    }

    public static void SaveTodos(TodoList todos, string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("Text,Status,LastUpdate");

            for (int i = 0; i < todos.Count; i++)
            {
                TodoItem item = todos.GetItem(i);
                string text = item.Text.Replace("\"", "\"\"");
                string status = item.IsDone ? "done" : "pending";
                string date = item.LastUpdate.ToString("yyyy-MM-dd HH:mm:ss");
                writer.WriteLine($"\"{text}\",{status},{date}");
            }
        }
    }

    public static TodoList LoadTodos(string filePath)
    {
        TodoList todoList = new TodoList();

        if (!File.Exists(filePath))
        {
            return todoList;
        }

        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = ParseCsvLine(line);

                if (parts.Length >= 3)
                {
                    string text = parts[0].Replace("\"\"", "\"");
                    string status = parts[1];
                    string dateString = parts[2];

                    TodoItem item = new TodoItem(text);

                    if (status.ToLower() == "done")
                    {
                        item.MarkDone();
                    }

                    todoList.Add(item);
                }
            }
        }

        return todoList;
    }