namespace TodoList.Exceptions;

public class DuplicateLoginException : Exception
{
	public string Login { get; }

	public DuplicateLoginException() : base("Логин уже занят.") { }

	public DuplicateLoginException(string login)
		: base($"Логин '{login}' уже используется.")
	{
		Login = login;
	}

	public DuplicateLoginException(string message, Exception innerException)
		: base(message, innerException) { }
}