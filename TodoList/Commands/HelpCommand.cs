
namespace TodoList.Commands;
public class HelpCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("""
        ---------------------------------------------------------
        help    - список команд.
        profile - данные пользователя.
        add "текст"   - добавить задачу.
        view - показать задачи.
        status <idx> <status>- присвоить задаче статус.
        delete <idx> - удалить задачу.
        update <idx> new text - обновить текст задачи.
        read <idx> - прочитать полный текст задачи.
        exit    - выход.

        ---------------------------------------------------------
        Индексы для команды view:
        --index / -i - индекс задачи.
        --statuses / -s - статусы.
        --update-date / -d - время, дата.
        --all / -a - показать всё.
        ---------------------------------------------------------
        Статусы для команды Status:
        NotStarted => не начато.
        InProgress => в процессе.
        Completed => выполнено.
        Postponed => отложено.
        Failed => провалено.
        """);
    }

	public void Unexecute()
	{

	}
}