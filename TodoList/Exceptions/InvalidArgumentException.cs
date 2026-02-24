namespace TodoList.Exceptions;

public class InvalidArgumentException : Exception
{
	public InvalidArgumentException() : base("Неверный аргумент команды.") { }

	public InvalidArgumentException(string message) : base(message) { }

	public InvalidArgumentException(string message, Exception innerException)
		: base(message, innerException) { }
}