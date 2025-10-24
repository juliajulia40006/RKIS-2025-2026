using System;

class Program
{
    static Profile userProfile;
    static TodoList todoList;

    static void Main(string[] args)
    {
        Console.WriteLine("The program was made by Deinega and Piyagova");

        Console.WriteLine("Tell me your name:");
        firstName = Console.ReadLine();

        Console.WriteLine("Tell me your surname:");
        surname = Console.ReadLine();

        Console.WriteLine("Tell me your year of birth:");
        birthYear = int.Parse(Console.ReadLine());

        userProfile = new Profile(firstName, surname, birthYear);
        todoList = new TodoLst();

        Console.WriteLine($"New user Added: {userProfile.GetInfo()}");

        Console.WriteLine("Добро пожаловать в todoList.");

        while (true)
        {
            Console.Write("\nВведите команду (help - список команд): ");
            string input = Console.ReadLine()?.Trim() ?? "";

            if (string.IsNullOrEmpty(input))
                continue;

            string[] parts = input.Split(' ', 2);
            string command = parts[0].ToLower();
            string argument = parts.Length > 1 ? parts[1] : "";

            switch (command)
            {
                case "help":
                    Help();
                    break;

                case "profile":
                    Profile();
                    break;

                case "add":
                    Add(argument);
                    break;

                case "view":
                    View(argument);
                    break;

                case "done":
                    Done(argument);
                    break;

                case "delete":
                    Delete(argument);
                    break;

                case "update":
                    Update(argument);
                    break;

                case "exit":
                    Console.WriteLine("Выход...");
                    return;

                case "read":
                    Read(argument, todos, taskCount, statuses, dates);
                    break;

                default:
                    Console.WriteLine("Неизвестная команда. Введите 'help' для справки.");
                    break;
            }
        }
    }

    static void Help()
    {
        Console.WriteLine("---------------------------------------------------------");
        Console.WriteLine("\nhelp    - список команд.");
        Console.WriteLine("profile - данные пользователя.");
        Console.WriteLine("add     - добавить задачу (add \"текст\").");
        Console.WriteLine("view - показать задачи.");
        Console.WriteLine("done <idx> - пометить задачу как выполненную.");
        Console.WriteLine("delete  - удаляет задачу.");
        Console.WriteLine("update \"idx\" new_text - обновляет текст задачи.");
        Console.WriteLine("read <idx> - прочитать полный текст задачи " );
        Console.WriteLine("exit    - выход.");
        Console.WriteLine("---------------------------------------------------------");
        Console.WriteLine("Индексы для команды view:");
        Console.WriteLine("--index / -i - индекс задачи.");
        Console.WriteLine("--statuses / -s - статусы.");
        Console.WriteLine("--update-date / -d - время, дата.");
        Console.WriteLine("--all / -a - показать всё.");
    }

    static void Profile()
    {
        Console.WriteLine($"\n{userProfile.GetInfo()}");
    }

    static void Add(string argument)
    {
        if (string.IsNullOrEmpty(argument))
        {
            Console.WriteLine("Ошибка: Используйте: add \"текст задачи\"");
            return;
        }

        if (argument == "--multiline" || argument == "-m")
        {

            Console.WriteLine("Многострочный режим. Вводите задачи (для завершения введите !end):");
            string multilineTask = "";
            string line;

            while (true)
            {
                Console.Write("> ");
                line = Console.ReadLine();

                if (line == "!end")
                {
                    break;
                }

                if (!string.IsNullOrEmpty(multilineTask))
                {
                    multilineTask += "\n";
                }
                multilineTask += line;
            }

            if (string.IsNullOrEmpty(multilineTask))
            {
                Console.WriteLine("Ошибка: Текст задачи не может быть пустым");
                return;
            }

            if (taskCount >= todos.Length)
            {
                ExpandArrays();
                Console.WriteLine($"Массив расширен до {todos.Length} элементов");
            }

            todos[taskCount] = multilineTask;
            dates[taskCount] = DateTime.Now;
            statuses[taskCount] = false;
            taskCount++;
            Console.WriteLine($" Добавлено многострочная задача");
        }
        else
        {
            string[] taskParts = argument.Split(new char[] { '"' }, StringSplitOptions.RemoveEmptyEntries);

            if (taskParts.Length == 0)
            {
                Console.WriteLine("Ошибка: Текст задачи не может быть пустым");
                return;
            }

            string task = taskParts[0].Trim();

            if (string.IsNullOrEmpty(task))
            {
                Console.WriteLine("Ошибка: Текст задачи не может быть пустым");
                return;
            }

            if (taskCount >= todos.Length)
            {
                ExpandArrays();
                Console.WriteLine($"Массив расширен до {todos.Length} элементов");
            }

            todos[taskCount] = task;
            dates[taskCount] = DateTime.Now;
            statuses[taskCount] = false;
            taskCount++;
            Console.WriteLine($" Добавлено: {task}");
        }
    }

    static void View(string argument)
    {
        if (taskCount == 0)
        {
            Console.WriteLine("\nЗадач нет");
            return;
        }

        bool showIndex = false;
        bool showStatus = false;
        bool showDate = false;
        bool showAll = false;

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
                            case 'i':
                                showIndex = true;
                                break;
                            case 's':
                                showStatus = true;
                                break;
                            case 'd':
                                showDate = true;
                                break;
                            case 'a':
                                showAll = true;
                                break;
                        }
                    }
                }
                else
                {
                    switch (cleanFlag)
                    {
                        case "--index":
                        case "-i":
                            showIndex = true;
                            break;
                        case "--status":
                        case "-s":
                            showStatus = true;
                            break;
                        case "--update-date":
                        case "-d":
                            showDate = true;
                            break;
                        case "--all":
                        case "-a":
                            showAll = true;
                            break;
                    }
                }
            }
        }

        if (showAll)
        {
            showIndex = true;
            showStatus = true;
            showDate = true;
        }

        if (!showIndex && !showStatus && !showDate)
        {
            Console.WriteLine("\nВаши задачи:");
            for (int i = 0; i < taskCount; i++)
            {
                string taskText = GetFirstLine(todos[i]);
                string shortTask = taskText.Length > 30 ? taskText.Substring(0, 27) + "..." : taskText;
                Console.WriteLine($"{shortTask}");
            }
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

        for (int i = 0; i < taskCount; i++)
        {
            string line = "";

            if (showIndex)
            {
                line += (i + 1).ToString().PadRight(indexWidth) + " ";
            }

            string taskText = GetFirstLine(todos[i]);
            string shortTask = taskText.Length > 30 ? taskText.Substring(0, 27) + "..." : taskText;
            line += shortTask.PadRight(taskWidth) + " ";

            if (showStatus)
            {
                string status = statuses[i] ? "выполнено" : "не выполнено";
                line += status.PadRight(statusWidth) + " ";
            }

            if (showDate)
            {
                line += dates[i].ToString("dd.MM.yyyy HH:mm").PadRight(dateWidth);
            }

            Console.WriteLine(line.TrimEnd());
        }
    }

    static string GetFirstLine(string task)
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


    static void Done(string argument)
    {
        if (int.TryParse(argument, out int taskIndex) && taskIndex >= 1 && taskIndex <= taskCount)
        {
            int index = taskIndex - 1;
            statuses[index] = true;
            dates[index] = DateTime.Now;
            Console.WriteLine($"Задача '{todos[index]}' отмечена как выполненная!");
        }
        else
        {
            Console.WriteLine("Неверный номер задачи!");
        }
    }

    static void Delete(string argument)
    {
        if (int.TryParse(argument, out int deleteIndex) && deleteIndex >= 1 && deleteIndex <= taskCount)
        {
            int index = deleteIndex - 1;
            string deletedTask = todos[index];

            for (int i = index; i < taskCount - 1; i++)
            {
                todos[i] = todos[i + 1];
                dates[i] = dates[i + 1];
                statuses[i] = statuses[i + 1];
            }

            taskCount--;
            Console.WriteLine($"Задача '{deletedTask}' удалена.");
        }
        else
        {
            Console.WriteLine("Неверный номер задачи!");
        }
    }

    static void Update(string argument)
    {
        string[] updateParts = argument.Split(new char[] { '"' }, StringSplitOptions.RemoveEmptyEntries);
        if (updateParts.Length >= 2 && int.TryParse(updateParts[0].Trim(), out int updateIndex) && updateIndex >= 1 && updateIndex <= taskCount)
        {
            int index = updateIndex - 1;
            string oldTask = todos[index];
            string newTask = updateParts[1].Trim();

            if (string.IsNullOrEmpty(newTask))
            {
                Console.WriteLine("Ошибка: Новый текст задачи не может быть пустым");
                return;
            }

            todos[index] = newTask;
            dates[index] = DateTime.Now;
            Console.WriteLine($"Задача обновлена: '{oldTask}' -> '{newTask}'");
        }
        else
        {
            Console.WriteLine("Ошибка: Используйте: update \"номер\" новый текст");
        }
    }

    static void ExpandArrays()
    {
        string[] newTodos = new string[todos.Length * 2];
        DateTime[] newDates = new DateTime[dates.Length * 2];
        bool[] newStatuses = new bool[dates.Length * 2];

        for (int i = 0; i < todos.Length; i++)
        {
            newTodos[i] = todos[i];
            newDates[i] = DateTime.Now;
            newStatuses[i] = statuses[i];
        }

        todos = newTodos;
        dates = newDates;
        statuses = newStatuses;
    }

    static void Read(string argument, string[] todos, int taskCount, bool[] statuses, DateTime[] dates)
    {
        if (int.TryParse(argument, out int taskIndex) && taskIndex >= 1 && taskIndex <= taskCount)
        {
            int index = taskIndex - 1;
            string status = statuses[index] ? "выполнено" : "не выполнено";
            Console.WriteLine($"\nЗадача #{taskIndex}:");
            Console.WriteLine($"Текст: \n{todos[index]}");
            Console.WriteLine($"Статус: {status}");
            Console.WriteLine($"Дата изменения: {dates[index]}");
        }
        else
        {
            Console.WriteLine("Неверный номер задачи!");
        }
    }
}