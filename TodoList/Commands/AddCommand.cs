using TodoList;
namespace TodoList.Commands;
public class AddCommand : ICommand
{
    public bool Multiline { get; set; } = false;
    public string TaskText { get; set; } = "";
    public TodoList TodoList { get; set; }

    public void Execute()
    {
        if (Multiline)
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
            TodoList.Add(item);
            Console.WriteLine("Добавлена многострочная задача");
        }
        else
        {
            if (string.IsNullOrEmpty(TaskText))
            {
                Console.WriteLine("Ошибка: Текст задачи не может быть пустым.");
                return;
            }

            TodoItem item = new TodoItem(TaskText);
            TodoList.Add(item);
            Console.WriteLine($"Добавлено: {TaskText}.");
        }
    }
}