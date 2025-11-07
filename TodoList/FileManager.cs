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
            writer.WriteLine("Index;Text;IsDone;LastUpdate");

            for (int i = 0; i < todos.Count; i++)
            {
                TodoItem item = todos.GetItem(i);
                string text = item.Text
                    .Replace("\"", "\"\"")
                    .Replace("\n", "\\n");
                string status = item.IsDone ? "true" : "false";
                string date = item.LastUpdate.ToString("yyyy-MM-ddTHH:mm:ss");
                writer.WriteLine($"{i};\"{text}\";{status};{date}");
            }
        }
    }

    public static TodoList LoadTodos(string filePath)
    {
        TodoList todoList = new TodoList();

        if (!File.Exists(filePath))
            return todoList;

        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            bool isFirstLine = true;

            while ((line = reader.ReadLine()) != null)
            {
                if (isFirstLine)
                {
                    isFirstLine = false;
                    continue;
                }

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = ParseCsvLine(line);
                if (parts.Length >= 4)
                {
                    string text = parts[1]
                        .Replace("\"\"", "\"")
                        .Replace("\\n", "\n");
                    bool isDone = parts[2].ToLower() == "true";
                    DateTime lastUpdate = DateTime.Parse(parts[3]);

                    TodoItem item = new TodoItem(text);
                    if (isDone) item.MarkDone();
                    todoList.Add(item);
                }
            }
        }
        return todoList;
    }

    private static string[] ParseCsvLine(string line)
    {
        int fieldCount = CountFields(line);
        string[] result = new string[fieldCount];
        int fieldIndex = 0;
        bool inQuotes = false;
        string currentField = "";

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ';' && !inQuotes)
            {
                result[fieldIndex] = currentField;
                currentField = "";
                fieldIndex++;
            }
            else
            {
                currentField += c;
            }
        }

        if (fieldIndex < result.Length)
        {
            result[fieldIndex] = currentField;
        }

        return result;
    }

    private static int CountFields(string line)
    {
        int count = 1;
        bool inQuotes = false;

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ';' && !inQuotes)
            {
                count++;
            }
        }

        return count;
    }
}