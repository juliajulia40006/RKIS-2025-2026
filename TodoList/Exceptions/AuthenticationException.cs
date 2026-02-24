namespace TodoList.Exceptions;

public class AuthenticationException : Exception
{
	public AuthenticationException() : base("Ошибка аутентификации.") { }

	public AuthenticationException(string message) : base(message) { }

	public AuthenticationException(string message, Exception innerException)
		: base(message, innerException) { }
}