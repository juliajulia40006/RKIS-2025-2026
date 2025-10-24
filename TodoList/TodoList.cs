public class TodoList
{
    private TodoItem[] items;
    private int count;

    public int Count => count;

    public TodoList()
    {
        items = new TodoItem[2];
        count = 0;
    }
}