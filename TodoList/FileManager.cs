using System;
using static TodoList.TodoItem;

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
        string[] lines = {
        profile.FirstName,
        profile.LastName,
        profile.BirthYear.ToString()
        };
        File.WriteAllLines(filePath, lines);
    }

    public static Profile LoadProfile(string filePath)
    {
        if (!File.Exists(filePath)) return null;

        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length < 3) return null;

        return new Profile(lines[0], lines[1], int.Parse(lines[2]));
    }

    public static void SaveTodos(TodoList todos, string filePath)
    {
        string[] lines = new string[todos.Count + 1];
        lines[0] = "Index;Text;Status;LastUpdate";

        for (int i = 0; i < todos.Count; i++)
        {
            TodoItem item = todos[i];
            lines[i + 1] = $"{i};\"{item.Text.Replace("\"", "\"\"").Replace("\n", "\\n")}\";{item.Status};{item.LastUpdate:yyyy-MM-ddTHH:mm:ss}";
        }

        File.WriteAllLines(filePath, lines);
    }

    public static TodoList LoadTodos(string filePath)
    {
        TodoList todoList = new TodoList();
        if (!File.Exists(filePath)) return todoList;

        string[] lines = File.ReadAllLines(filePath);

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] parts = ParseCsvLine(lines[i]);
            if (parts.Length < 4) continue;

            string text = parts[1].Replace("\"\"", "\"").Replace("\\n", "\n").Trim('"');
            TodoStatus status;	
            try
            {
                status = Enum.Parse<TodoStatus>(parts[2]);
            }
            catch (ArgumentException)
            {
                status = TodoStatus.NotStarted;
            }


            DateTime lastUpdate;
            if (!DateTime.TryParse(parts[3], out lastUpdate))
            {
                lastUpdate = DateTime.Now;
            }

            TodoItem item = new TodoItem(text);
            item.SetStatus(status);
            item.SetLastUpdate(lastUpdate);

            todoList.Add(item);
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