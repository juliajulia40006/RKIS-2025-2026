using TodoList;
using static TodoList.TodoItem;

namespace TodoList.Commands;

public static class CommandParser
{
    public static ICommand Parse(string inputString, List<TodoItem> todoItems, Profile profile)

    {
        if (string.IsNullOrEmpty(inputString))
            return null;

        string[] parts = inputString.Split(' ', 2);
        string command = parts[0].ToLower();
        string argument = parts.Length > 1 ? parts[1] : "";

        switch (command)
        {
            case "help":
                return new HelpCommand();

			case "profile":
				return ParseProfileCommand(argument, profile);

			case "add":
                return ParseAddCommand(argument, todoItems);

            case "view":
                return ParseViewCommand(argument, todoItems);

            case "status":
                return ParseStatusCommand(argument, todoItems);

            case "delete":
                return ParseDeleteCommand(argument, todoItems);

            case "update":
                return ParseUpdateCommand(argument, todoItems);

            case "read":
                return ParseReadCommand(argument, todoItems);

			case "undo":
				return new UndoCommand();
			case "redo":
				return new RedoCommand();

			case "exit":
                return new ExitCommand();

            default:
                Console.WriteLine("Неизвестная команда. Введите 'help' для справки.");
                return null;
        }
    }
	private static ICommand ParseProfileCommand(string argument, Profile profile)
	{
		var command = new ProfileCommand { Profile = profile };

		if (!string.IsNullOrEmpty(argument))
		{
			string[] flags = argument.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			foreach (string flag in flags)
			{
				string cleanFlag = flag.Trim().ToLower();
				if (cleanFlag == "-o" || cleanFlag == "--out")
				{
					return new LogoutCommand();
				}
			}
		}

		return command;
	}
	private static ICommand ParseAddCommand(string argument, List<TodoItem> todoItems)
	{
        var command = new AddCommand { TodoItems = todoItems };

        if (string.IsNullOrEmpty(argument))
        {
            command.Multiline = false;
            command.TaskText = "";
            return command;
        }

        string cleanArgument = argument.Trim().ToLower();

        if (cleanArgument == "--multiline" || cleanArgument == "-m")
        {
            command.Multiline = true;
        }
        else
        {
            command.Multiline = false;
            command.TaskText = argument.Trim();
        }

        return command;
    }

    private static ICommand ParseViewCommand(string argument, List<TodoItem> todoItems)
	{
        var command = new ViewCommand { TodoItems = todoItems };

        if (!string.IsNullOrEmpty(argument))
        {
            string[] flags = argument.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (string flag in flags)
            {
                string cleanFlag = flag.Trim().ToLower();

                if (cleanFlag.StartsWith("-") && cleanFlag.Length > 2 && !cleanFlag.StartsWith("--"))
                {
                    foreach (char c in cleanFlag.Substring(1))
                    {
                        switch (c)
                        {
                            case 'i': command.ShowIndex = true; break;
                            case 's': command.ShowStatus = true; break;
                            case 'd': command.ShowDate = true; break;
                            case 'a': command.ShowAll = true; break;
                        }
                    }
                }
                else
                {
                    switch (cleanFlag)
                    {
                        case "--index": case "-i": command.ShowIndex = true; break;
                        case "--status": case "-s": command.ShowStatus = true; break;
                        case "--update-date": case "-d": command.ShowDate = true; break;
                        case "--all": case "-a": command.ShowAll = true; break;
                    }
                }
            }
        }

        return command;
    }

    private static ICommand ParseStatusCommand(string argument, List<TodoItem> todoItems)
	{
        if (string.IsNullOrEmpty(argument))
        {
            Console.WriteLine("Ошибка: Используйте: status <номер> <статус>");
            return new StatusCommand { TodoItems = todoItems };
        }

        string[] parts = argument.Split(' ', 2);
        if (parts.Length < 2)
        {
            Console.WriteLine("Ошибка: Используйте: status <номер> <статус>");
            return new StatusCommand { TodoItems = todoItems };
        }

        if (int.TryParse(parts[0], out int taskIndex))
        {
            string statusStr = parts[1];
            TodoStatus status = Enum.Parse<TodoStatus>(statusStr, true);

            if (status == TodoStatus.NotStarted && statusStr != "notstarted")
            {
                Console.WriteLine($"Ошибка: Неизвестный статус '{parts[1]}'. Допустимые статусы: notstarted, inprogress, completed, postponed, failed");
                return new StatusCommand { TodoItems = todoItems };
            }

            return new StatusCommand
            {
				TodoItems = todoItems,
                TaskIndex = taskIndex,
                Status = status
            };
        }

        Console.WriteLine("Ошибка: Используйте: status <номер> <статус>");
        return new StatusCommand { TodoItems = todoItems };
    }

    private static ICommand ParseDeleteCommand(string argument, List<TodoItem> todoItems)
	{
        if (int.TryParse(argument, out int taskIndex))
        {
            return new DeleteCommand { TodoItems = todoItems, TaskIndex = taskIndex };
        }
        return new DeleteCommand { TodoItems = todoItems };
    }

    private static ICommand ParseUpdateCommand (string argument, List<TodoItem> todoItems)
	{
        if (string.IsNullOrEmpty(argument))
        {
            Console.WriteLine("Ошибка: Используйте: update <номер> новый текст");
            return new UpdateCommand { TodoItems = todoItems };
        }

        int firstSpaceIndex = argument.IndexOf(' ');
        if (firstSpaceIndex <= 0)
        {
            Console.WriteLine("Ошибка: Используйте: update <номер> новый текст");
            return new UpdateCommand { TodoItems = todoItems };
        }

        string indexPart = argument.Substring(0, firstSpaceIndex).Trim();
        string textPart = argument.Substring(firstSpaceIndex + 1).Trim();

        if (int.TryParse(indexPart, out int updateIndex) && !string.IsNullOrEmpty(textPart))
        {
            return new UpdateCommand
            {
				TodoItems = todoItems,
                TaskIndex = updateIndex,
                NewText = textPart
            };
        }

        Console.WriteLine("Ошибка: Используйте: update <номер> новый текст");
        return new UpdateCommand { TodoItems = todoItems };
    }

    private static ICommand ParseReadCommand(string argument, List<TodoItem> todoItems)
{
        if (int.TryParse(argument, out int taskIndex))
        {
            return new ReadCommand { TodoItems = todoItems, TaskIndex = taskIndex };
        }
        return new ReadCommand { TodoItems = todoItems };
    }
}