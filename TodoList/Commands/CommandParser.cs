namespace TodoList.Commands;

public static class CommandParser
{
    public static ICommand  Parse(string inputString, TodoList todolist, Profile profile)
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
                return new ProfileCommand { Profile = profile };

            case "add":
                return ParseAddCommand(argument, todolist);

            case "view":
                return ParseViewCommand(argument, todolist);

            case "done":
                return ParseDoneCommand(argument, todolist);

            case "delete":
                return ParseDeleteCommand(argument, todolist);

            case "update":
                return ParseUpdateCommand(argument, todolist);

            case "read":
                return ParseReadCommand(argument, todolist);

            case "exit":
                return new ExitCommand();

            default:
                Console.WriteLine("Неизвестная команда. Введите 'help' для справки.");
                return null;
        }
    }

    private static ICommand ParseAddCommand(string argument, TodoList todolist)
    {
        var command = new AddCommand { TodoList = todolist };

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

    private static ICommand ParseViewCommand(string argument, TodoList todolist)
    {
        var command = new ViewCommand { TodoList = todolist };

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

    private static ICommand ParseDoneCommand(string argument, TodoList todolist)
    {
        if (int.TryParse(argument, out int taskIndex))
        {
            return new DoneCommand { TodoList = todolist, TaskIndex = taskIndex };
        }
        return new DoneCommand { TodoList = todolist };
    }

    private static ICommand ParseDeleteCommand(string argument, TodoList todolist)
    {
        if (int.TryParse(argument, out int taskIndex))
        {
            return new DeleteCommand { TodoList = todolist, TaskIndex = taskIndex };
        }
        return new DeleteCommand { TodoList = todolist };
    }

    private static ICommand ParseUpdateCommand(string argument, TodoList todolist)
    {
        if (string.IsNullOrEmpty(argument))
        {
            Console.WriteLine("Ошибка: Используйте: update <номер> новый текст");
            return new UpdateCommand { TodoList = todolist };
        }

        int firstSpaceIndex = argument.IndexOf(' ');
        if (firstSpaceIndex <= 0)
        {
            Console.WriteLine("Ошибка: Используйте: update <номер> новый текст");
            return new UpdateCommand { TodoList = todolist };
        }

        string indexPart = argument.Substring(0, firstSpaceIndex).Trim();
        string textPart = argument.Substring(firstSpaceIndex + 1).Trim();

        if (int.TryParse(indexPart, out int updateIndex) && !string.IsNullOrEmpty(textPart))
        {
            return new UpdateCommand
            {
                TodoList = todolist,
                TaskIndex = updateIndex,
                NewText = textPart
            };
        }

        Console.WriteLine("Ошибка: Используйте: update <номер> новый текст");
        return new UpdateCommand { TodoList = todolist };
    }

    private static ICommand ParseReadCommand(string argument, TodoList todolist)
    {
        if (int.TryParse(argument, out int taskIndex))
        {
            return new ReadCommand { TodoList = todolist, TaskIndex = taskIndex };
        }
        return new ReadCommand { TodoList = todolist };
    }
}