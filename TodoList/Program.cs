using System;

class Task // Убрана точка с запятой после имени класса
{
    static void Main(string[] args)
    {
        Console.WriteLine( "the programm was made by Deinega and Piyagova" );

        Console.WriteLine("Tell me your name:");
        string fname = Console.ReadLine();

        Console.WriteLine("Tell me your surname:");
        string sname = Console.ReadLine();

        Console.WriteLine("Tell me your year of birth:");

       
        int birthYear = int.Parse(Console.ReadLine());
        DateTime currentDate = DateTime.Today;
        int age = currentDate.Year - birthYear;

        
        Console.WriteLine($"New user Added: {fname} {sname}, Age: {age}");
    }
}
