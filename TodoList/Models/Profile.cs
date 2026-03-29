namespace TodoList.Models;
using System.ComponentModel.DataAnnotations;

public class Profile
{
	[Key]
	public Guid Id { get; private set; }
	[Required]
	public string Login { get; private set; }
	[Required]
	public string Password { get; private set; }
	[Required]
	public string FirstName { get; private set; }
	[Required]
    public string LastName { get; private set; }
    public int BirthYear { get; private set; }

	public ICollection<TodoItem> Todos { get; set; } = new List<TodoItem>();

	public Profile(Guid id, string login, string password, string firstName, string lastName, int birthYear)
    {
		Id = id;
		Login = login;
		Password = password;
		FirstName = firstName;
        LastName = lastName;
        BirthYear = birthYear;
    }

    public string GetInfo()
    {	
        int age = DateTime.Today.Year - BirthYear;
        return $"{FirstName} {LastName}, возраст {age}";
    }
}