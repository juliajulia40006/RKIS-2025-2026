using TodoList;
using static TodoList.TodoItem;

namespace TodoList.Commands;

public static class CommandParser
{
	private static Dictionary<string, Func<string, List<TodoItem>, Profile, ICommand>> _commandHandlers = new();

	static CommandParser()
	{
		_commandHandlers["help"] = ParseHelpCommand;
		_commandHandlers["profile"] = ParseProfileCommand;
		_commandHandlers["add"] = ParseAddCommand;
		_commandHandlers["view"] = ParseViewCommand;
		_commandHandlers["status"] = ParseStatusCommand;
		_commandHandlers["delete"] = ParseDeleteCommand;
		_commandHandlers["update"] = ParseUpdateCommand;
		_commandHandlers["read"] = ParseReadCommand;
		_commandHandlers["undo"] = ParseUndoCommand;
		_commandHandlers["redo"] = ParseRedoCommand;
		_commandHandlers["exit"] = ParseExitCommand;
		_commandHandlers["search"] = ParseSearchCommand;
	}

	public static ICommand Parse(string inputString, List<TodoItem> todoItems, Profile profile)

    {
        if (string.IsNullOrEmpty(inputString))
            return null;

        string[] parts = inputString.Split(' ', 2);
        string command = parts[0].ToLower();
        string argument = parts.Length > 1 ? parts[1] : "";

		if (_commandHandlers.TryGetValue(command, out var handler))
			return handler(argument, todoItems, profile);

		Console.WriteLine("Неизвестная команда. Введите 'help' для справки.");
		return null;

	}
	private static ICommand ParseHelpCommand(string argument, List<TodoItem> todoItems, Profile profile)
	{
		return new HelpCommand();
	}

	private static ICommand ParseProfileCommand(string argument, List<TodoItem> todoItems, Profile profile)
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
	private static ICommand ParseAddCommand(string argument, List<TodoItem> todoItems, Profile profile)
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

    private static ICommand ParseViewCommand(string argument, List<TodoItem> todoItems, Profile profile)
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

    private static ICommand ParseStatusCommand(string argument, List<TodoItem> todoItems, Profile profile)
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

    private static ICommand ParseDeleteCommand(string argument, List<TodoItem> todoItems, Profile profile)
	{
        if (int.TryParse(argument, out int taskIndex))
        {
            return new DeleteCommand { TodoItems = todoItems, TaskIndex = taskIndex };
        }
        return new DeleteCommand { TodoItems = todoItems };
    }

    private static ICommand ParseUpdateCommand (string argument, List<TodoItem> todoItems, Profile profile)
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

    private static ICommand ParseReadCommand(string argument, List<TodoItem> todoItems, Profile profile)
{
        if (int.TryParse(argument, out int taskIndex))
        {
            return new ReadCommand { TodoItems = todoItems, TaskIndex = taskIndex };
        }
        return new ReadCommand { TodoItems = todoItems };
    }

	private static ICommand ParseUndoCommand(string args, List<TodoItem> todoItems, Profile profile)
	{
		return new UndoCommand();
	}

	private static ICommand ParseRedoCommand(string args, List<TodoItem> todoItems, Profile profile)
	{
		return new RedoCommand();
	}

	private static ICommand ParseExitCommand(string args, List<TodoItem> todoItems, Profile profile)
	{
		return new ExitCommand();
	}

	private static ICommand ParseSearchCommand(string argument, List<TodoItem> todoItems, Profile profile)
	{
		var command = new SearchCommand { TodoItems = todoItems };

		if (string.IsNullOrEmpty(argument))
			return command;

		string[] parts = argument.Split(' ');

		for (int i = 0; i < parts.Length; i++)
		{
			string flag = parts[i].ToLower();

			switch (flag)
			{
				case "--contains":
					if (i + 1 < parts.Length)
					{
						// Просто берем следующий элемент как текст
						command.ContainsText = parts[++i];
					}
					break;

				case "--starts-with":
					if (i + 1 < parts.Length)
						command.StartsWithText = parts[++i];
					break;

				case "--ends-with":
					if (i + 1 < parts.Length)
						command.EndsWithText = parts[++i];
					break;

				case "--from":
					if (i + 1 < parts.Length)
					{
						if (DateTime.TryParse(parts[i + 1], out DateTime fromDate))
							command.FromDate = fromDate;
						else
							Console.WriteLine($"Ошибка: Неверный формат даты '{parts[i + 1]}'");
						i++;
					}
					break;

				case "--to":
					if (i + 1 < parts.Length)
					{
						if (DateTime.TryParse(parts[i + 1], out DateTime toDate))
							command.ToDate = toDate;
						else
							Console.WriteLine($"Ошибка: Неверный формат даты '{parts[i + 1]}'");
						i++;
					}
					break;

				case "--status":
					if (i + 1 < parts.Length)
					{
						if (Enum.TryParse<TodoStatus>(parts[i + 1], true, out TodoStatus status))
							command.Status = status;
						else
							Console.WriteLine($"Ошибка: Неверный статус '{parts[i + 1]}'");
						i++;
					}
					break;

				case "--sort":
					if (i + 1 < parts.Length)
					{
						string sortValue = parts[i + 1].ToLower();
						if (sortValue == "text" || sortValue == "date")
						{
							if (string.IsNullOrEmpty(command.SortBy))
								command.SortBy = sortValue;
							else
								command.ThenBy = sortValue;
						}
						else
							Console.WriteLine($"Ошибка: Неверное значение сортировки '{parts[i + 1]}'");
						i++;
					}
					break;

				case "--desc":
					command.SortDesc = true;
					break;

				case "--top":
					if (i + 1 < parts.Length)
					{
						if (int.TryParse(parts[i + 1], out int top) && top > 0)
							command.Top = top;
						else
							Console.WriteLine($"Ошибка: Неверное значение top '{parts[i + 1]}'");
						i++;
					}
					break;
			}
		}

		return command;
	}
}