namespace TodoList.Exceptions;

public class ProfileNotFoundException : Exception
{
	public string Login { get; }

	public ProfileNotFoundException() : base("Профиль не найден.") { }
	public ProfileNotFoundException(string login)
		: base($"Профиль с логином '{login}' не найден.")
	{
		Login = login;
	}

	public ProfileNotFoundException(string message, Exception innerException)
		: base(message, innerException) { }
}