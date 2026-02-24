namespace TodoList.Exceptions;

public class InvalidCommandException : Exception
{
	public InvalidCommandException() : base("Неизвестная команда.") { }

	public InvalidCommandException(string message) : base(message) { }

	public InvalidCommandException(string message, Exception innerException)
		: base(message, innerException) { }
}