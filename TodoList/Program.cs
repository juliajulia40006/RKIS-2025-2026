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

            
        }
    }
}