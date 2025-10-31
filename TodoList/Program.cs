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

            ICommand command = CommandParser.Parse(input, todoList, userProfile);
            command?.Execute();
        }
    }
}

   