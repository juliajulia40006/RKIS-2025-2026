using Microsoft.VisualBasic.FileIO;
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

        bool[] statuses = new bool[2]; 
        
        DateTime[] dates = new DateTime[2];

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
                    Console.WriteLine("\nhelp    - список команд.");
                    Console.WriteLine("profile - данные пользователя.");
                    Console.WriteLine("add     - добавить задачу (add \"текст\").");
                    Console.WriteLine("view    - показать задачи.");
                    Console.WriteLine("done <idx> - пометить задачу как выполненную.");
                    Console.WriteLine("delete    - удаляет задачу.");
                    Console.WriteLine("update <idx> 'new_text' - обновляет текст задачи.");
                    Console.WriteLine("exit    - выход.");
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
                        DateTime[] newDates = new DateTime[dates.Length * 2];
                        bool[] newStatuses = new bool[dates.Length * 2];

                        // Через цикл перезаписываем в него содержимое текущего массива
                        for (int i = 0; i < todos.Length; i++)
                        {
                            newTodos[i] = todos[i];
                            newDates[i] = DateTime.Now;
                        }

                        // Записываем в переменную todos новый массив
                        todos = newTodos;
                        dates = newDates;
                        statuses = newStatuses;
                        Console.WriteLine($"Массив расширен до {todos.Length} элементов");

                       
                    }

                    // Добавляем задачу
                    todos[taskCount] = task;
                    dates[taskCount] = DateTime.Now;
                    statuses[taskCount] = false;
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
                            string status = statuses[i] ? "выполнено" : "не выполнено";
                            Console.WriteLine($"{i + 1}. {todos[i]} {dates[i]} {status} ");
                        }
                    }
                    break;

                case "done":
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
                    break;

                case "delete":
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
                    break;

                case "update":
                    string[] updateParts = argument.Split(new char[] { '"' }, StringSplitOptions.RemoveEmptyEntries);
                    if (updateParts.Length >= 2 && int.TryParse(updateParts[0].Trim(), out int updateIndex) && updateIndex >= 1 && updateIndex <= taskCount)
                    {
                        int index = updateIndex - 1;
                        string oldTask = todos[index];
                        string newTask = updateParts[1].Trim();

                        // Проверяем, что новый текст не пустой
                        if (string.IsNullOrEmpty(newTask))
                        {
                            Console.WriteLine("Ошибка: Новый текст задачи не может быть пустым");
                            break;
                        }

                        todos[index] = newTask;
                        dates[index] = DateTime.Now;
                        Console.WriteLine($"Задача обновлена: '{oldTask}' -> '{newTask}'");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: Используйте: update \"номер\" новый текст");
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