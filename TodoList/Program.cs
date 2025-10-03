using System;

class TodoList
{
    static void Main(string[] args)
    {
        Console.WriteLine("The program was made by Deinega and Piyagova");

        Console.WriteLine("Tell me your name:");
        string first_name = Console.ReadLine();

        Console.WriteLine("Tell me your surname:");
        string surname = Console.ReadLine();

        Console.WriteLine("Tell me your year of birth:");
        int birthYear = int.Parse(Console.ReadLine());

        DateTime currentDate = DateTime.Today;
        int age = currentDate.Year - birthYear;

        Console.WriteLine($"New user Added: {first_name} {surname}, Age: {age}");

        // Массив для хранения задач - начальная длина 2 элемента
        string[] todos = new string[2];
        int taskCount = 0;

        Console.WriteLine("Добро пожаловать в todoList.");

        // Бесконечный цикл для обработки команд
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
                    Console.WriteLine("\nhelp    - список команд");
                    Console.WriteLine("profile - данные пользователя");
                    Console.WriteLine("add     - добавить задачу (add \"текст\")");
                    Console.WriteLine("view    - показать задачи");
                    Console.WriteLine("exit    - выход");
                    break;

                case "profile":
                    int currentAge = DateTime.Now.Year - birthYear;
                    Console.WriteLine($"\n{first_name} {surname}, {birthYear} год рождения ({currentAge} лет)");
                    break;

                case "add":
                    if (string.IsNullOrEmpty(argument))
                    {
                        Console.WriteLine("Ошибка: Используйте: add \"текст задачи\"");
                        break;
                    }

                    // Используем String.Split для извлечения текста задачи
                    string[] taskParts = argument.Split(new char[] { '"' }, StringSplitOptions.RemoveEmptyEntries);

                    if (taskParts.Length == 0)
                    {
                        Console.WriteLine("Ошибка: Текст задачи не может быть пустым");
                        break;
                    }

                    string task = taskParts[0].Trim();

                    if (string.IsNullOrEmpty(task))
                    {
                        Console.WriteLine("Ошибка: Текст задачи не может быть пустым");
                        break;
                    }

                    // Проверяем, хватит ли места в массиве для новой задачи
                    if (taskCount >= todos.Length)
                    {
                        // Создаем новый массив в 2 раза длиннее текущего
                        string[] newTodos = new string[todos.Length * 2];

                        // Через цикл перезаписываем в него содержимое текущего массива
                        for (int i = 0; i < todos.Length; i++)
                        {
                            newTodos[i] = todos[i];
                        }

                        // Записываем в переменную todos новый массив
                        todos = newTodos;
                        Console.WriteLine($"Массив расширен до {todos.Length} элементов");
                    }

                    // Добавляем задачу
                    todos[taskCount] = task;
                    taskCount++;
                    Console.WriteLine($" Добавлено: {task}");
                    break;

                case "view":
                    if (taskCount == 0)
                    {
                        Console.WriteLine("\nЗадач нет");
                    }
                    else
                    {
                        Console.WriteLine("\nВаши задачи:");
                        for (int i = 0; i < taskCount; i++)
                        {
                            Console.WriteLine($"{i + 1}. {todos[i]}");
                        }
                    }
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
}