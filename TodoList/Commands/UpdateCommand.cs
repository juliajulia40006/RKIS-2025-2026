using TodoList;
namespace TodoList.Commands;
public class UpdateCommand : ICommand
{
    public int TaskIndex { get; set; }
    public string NewText { get; set; } = "";
    public TodoList TodoList { get; set; }

    public void Execute()
    {
        if (TaskIndex >= 1 && TaskIndex <= TodoList.Count)
        {
            int index = TaskIndex - 1;
            TodoItem item = TodoList.GetItem(index);
            string oldTask = item.Text;

            if (string.IsNullOrEmpty(NewText))
            {
                Console.WriteLine("Ошибка: Новый текст задачи не может быть пустым");
                return;
            }

            item.UpdateText(NewText);
            Console.WriteLine($"Задача обновлена: '{oldTask}' -> '{NewText}'");
        }
        else
        {
            Console.WriteLine("Ошибка: Используйте: update \"номер\" новый текст");
        }
    }
}