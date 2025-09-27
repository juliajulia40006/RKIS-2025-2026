using System;

class TodoList
{
    static void Main(string[] args)
    {
        Console.WriteLine( "the programm was made by Deinega and Piyagova" );

        Console.WriteLine("Tell me your name:");
        string first_name = Console.ReadLine();

        Console.WriteLine("Tell me your surname:");
        string surname = Console.ReadLine();

        Console.WriteLine("Tell me your year of birth:");

       
        int birthYear = int.Parse(Console.ReadLine());
        DateTime currentDate = DateTime.Today;
        int age = currentDate.Year - birthYear;

        
        Console.WriteLine($"New user Added: {first_name} {surname}, Age: {age}");
    }
}
