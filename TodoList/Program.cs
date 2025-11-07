using System;
using TodoList.Commands;
using TodoList;
using MainTodoList=TodoList.TodoList;
using System.IO;

class Program
{
    static Profile userProfile;
    static MainTodoList todoList;
    static string dataDirectory = "data";
    static string profileFilePath = Path.Combine("data", "profile.txt");
    static string todosFilePath = Path.Combine("data", "todo.csv");

    static void Main(string[] args)
    {
        if (userProfile == null)
        {
            Console.WriteLine("The program was made by Deinega and Piyagova");

            Console.WriteLine("Tell me your name:");
            string firstName = Console.ReadLine();

            Console.WriteLine("Tell me your surname:");
            string surname = Console.ReadLine();

            Console.WriteLine("Tell me your year of birth:");
            int birthYear = int.Parse(Console.ReadLine());

            userProfile = new Profile(firstName, surname, birthYear);
            todoList = new MainTodoList();
            FileManager.SaveProfile(userProfile, profileFilePath);

            Console.WriteLine($"New user Added: {userProfile.GetInfo()}");
        }

        todoList = FileManager.LoadTodos(todosFilePath);

        if (todoList.Count > 0)
        {
            Console.WriteLine($"\nЗагружено {todoList.Count} задач из сохранения.");
        }
        else
        {
            Console.WriteLine("\nСохраненных задач не найдено.");
        }

        Console.WriteLine("Добро пожаловать в todoList.");

        while (true)
        {
            Console.Write("\nВведите команду (help - список команд): ");
            string input = Console.ReadLine()?.Trim() ?? "";

            if (string.IsNullOrEmpty(input))
                continue;

            ICommand command = CommandParser.Parse(input, todoList, userProfile);

            if (command is ExitCommand)
            {
                FileManager.SaveProfile(userProfile, profileFilePath);
                FileManager.SaveTodos(todoList, todosFilePath);
                command.Execute();
                break;
            }
            command?.Execute();

            FileManager.SaveProfile(userProfile, profileFilePath);
            FileManager.SaveTodos(todoList, todosFilePath);
        }
    }
}

   