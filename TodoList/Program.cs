﻿using System;

class Program
{
    static Profile userProfile;
    static TodoList todoList;

    static void Main(string[] args)
    {
        Console.WriteLine("The program was made by Deinega and Piyagova");

        Console.WriteLine("Tell me your name:");
        string firstName = Console.ReadLine();

        Console.WriteLine("Tell me your surname:");
        string surname = Console.ReadLine();

        Console.WriteLine("Tell me your year of birth:");
        int birthYear = int.Parse(Console.ReadLine());

        userProfile = new Profile(firstName, surname, birthYear);
        todoList = new TodoList();

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
                    Read(argument);
                    break;

                default:
                    Console.WriteLine("Неизвестная команда. Введите 'help' для справки.");
                    break;
            }
        }
    }

    static void Help()
    {
        Console.WriteLine("""
            ---------------------------------------------------------
        help    - список команд.
        profile - данные пользователя.
        add "текст"   - добавить задачу.
        view - показать задачи.
        done <idx> - пометить задачу как выполненную.
        delete  - удалить задачу.
        update "idx" new text - обновить текст задачи.
        read <idx> - прочитать полный текст задачи.
        exit    - выход.
        ---------------------------------------------------------
        Индексы для команды view:
        --index / -i - индекс задачи.
        --statuses / -s - статусы.
        --update-date / -d - время, дата.
        --all / -a - показать всё.
        """);
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
                Console.WriteLine("Ошибка: Текст задачи не может быть пустым.");
                return;
            }

            TodoItem item = new TodoItem(multilineTask);
            todoList.Add(item);
            Console.WriteLine($" Добавлена многострочная задача");
        }
        else
        {
            string[] taskParts = argument.Split(new char[] { '"' }, StringSplitOptions.RemoveEmptyEntries);

            if (taskParts.Length == 0)
            {
                Console.WriteLine("Ошибка: Текст задачи не может быть пустым.");
                return;
            }

            string task = taskParts[0].Trim();

            if (string.IsNullOrEmpty(task))
            {
                Console.WriteLine("Ошибка: Текст задачи не может быть пустым.");
                return;
            }

            TodoItem item = new TodoItem(task);
            todoList.Add(item);
            Console.WriteLine($" Добавлено: {task}.");
        }
    }

    static void View(string argument)
    {
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
                            case 'i': showIndex = true; break;
                            case 's': showStatus = true; break;
                            case 'd': showDate = true; break;
                            case 'a': showAll = true; break;
                        }
                    }
                }
                else
                {
                    switch (cleanFlag)
                    {
                        case "--index": case "-i": showIndex = true; break;
                        case "--status": case "-s": showStatus = true; break;
                        case "--update-date": case "-d": showDate = true; break;
                        case "--all": case "-a": showAll = true; break;
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

        todoList.View(showIndex, showStatus, showDate);
    }


    static void Done(string argument)
    {
        if (int.TryParse(argument, out int taskIndex) && taskIndex >= 1 && taskIndex <= todoList.Count)
        {
            int index = taskIndex - 1;
            TodoItem item = todoList.GetItem(index);
            item.MarkDone();
            Console.WriteLine($"Задача '{item.Text}' отмечена как выполненная!");
        }
        else
        {
            Console.WriteLine("Неверный номер задачи!");
        }
    }

    static void Delete(string argument)
    {
        if (int.TryParse(argument, out int deleteIndex) && deleteIndex >= 1 && deleteIndex <= todoList.Count)
        {
            int index = deleteIndex - 1;
            TodoItem item = todoList.GetItem(index);
            string deletedTask = item.Text;

            todoList.Delete(index);
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
        if (updateParts.Length >= 2 && int.TryParse(updateParts[0].Trim(), out int updateIndex) && updateIndex >= 1 && updateIndex <= todoList.Count)
        {
            int index = updateIndex - 1;
            TodoItem item = todoList.GetItem(index);
            string oldTask = item.Text;
            string newTask = updateParts[1].Trim();

            if (string.IsNullOrEmpty(newTask))
            {
                Console.WriteLine("Ошибка: Новый текст задачи не может быть пустым");
                return;
            }

            item.UpdateText(newTask);
            Console.WriteLine($"Задача обновлена: '{oldTask}' -> '{newTask}'");
        }
        else
        {
            Console.WriteLine("Ошибка: Используйте: update \"номер\" новый текст");
        }
    }


    static void Read(string argument)
    {
        if (int.TryParse(argument, out int taskIndex) && taskIndex >= 1 && taskIndex <= todoList.Count)
        {
            int index = taskIndex - 1;
            TodoItem item = todoList.GetItem(index);
            Console.WriteLine($"Задача #{taskIndex}:");
            Console.WriteLine(item.GetFullInfo());
        }
        else
        {
            Console.WriteLine("Неверный номер задачи!");
        }
    }
}