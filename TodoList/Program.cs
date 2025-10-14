using System;

class TodoList
{
    static string first_name;
    static string surname;
    static int birthYear;
    static int age;

    static string[] todos = new string[2];
    static int taskCount = 0;
    static bool[] statuses = new bool[2];
    static DateTime[] dates = new DateTime[2];

    static void Main(string[] args)
    {
        Console.WriteLine("The program was made by Deinega and Piyagova");

        Console.WriteLine("Tell me your name:");
        first_name = Console.ReadLine();

        Console.WriteLine("Tell me your surname:");
        surname = Console.ReadLine();

        Console.WriteLine("Tell me your year of birth:");
        birthYear = int.Parse(Console.ReadLine());

        DateTime currentDate = DateTime.Today;
        age = currentDate.Year - birthYear;

        Console.WriteLine($"New user Added: {first_name} {surname}, Age: {age}");

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
                    View();
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

                default:
                    Console.WriteLine("Неизвестная команда. Введите 'help' для справки.");
                    break;
            }
        }
    }

    static void Help()
    {
        Console.WriteLine("\nhelp    - список команд.");
        Console.WriteLine("profile - данные пользователя.");
        Console.WriteLine("add     - добавить задачу (add \"текст\").");
        Console.WriteLine("view    - показать задачи.");
        Console.WriteLine("done <idx> - пометить задачу как выполненную.");
        Console.WriteLine("delete    - удаляет задачу.");
        Console.WriteLine("update \"idx\" new_text - обновляет текст задачи.");
        Console.WriteLine("exit    - выход.");
    }

    static void Profile()
    {
        Console.WriteLine($"\n{first_name} {surname}, {birthYear} год рождения ({age} лет)");
    }

    static void Add(string argument)
    {
        if (string.IsNullOrEmpty(argument))
        {
            Console.WriteLine("Ошибка: Используйте: add \"текст задачи\"");
            return;
        }

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

    static void View()
    {
        if (taskCount == 0)
        {
            Console.WriteLine("\nЗадач нет");
        }
        else
        {
            Console.WriteLine("\nВаши задачи:");
            for (int i = 0; i < taskCount; i++)
            {
                string status = statuses[i] ? "выполнено" : "не выполнено";
                Console.WriteLine($"{i + 1}. {todos[i]} {dates[i]} {status} ");
            }
        }
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
}