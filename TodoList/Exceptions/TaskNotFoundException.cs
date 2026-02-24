namespace TodoList.Exceptions;

public class TaskNotFoundException : Exception
{
	public int TaskIndex { get; }

	public TaskNotFoundException() : base("Задача не найдена.") { }

	public TaskNotFoundException(string message) : base(message) { }

	public TaskNotFoundException(int taskIndex)
		: base($"Задача с индексом {taskIndex} не существует.")
	{
		TaskIndex = taskIndex;
	}

	public TaskNotFoundException(string message, Exception innerException)
		: base(message, innerException) { }
}