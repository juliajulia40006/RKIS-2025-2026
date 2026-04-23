using TodoList.Models;
using Xunit;

namespace TodoList.Tests;

public class ProfileTests
{
	[Fact]
	public void GetInfo_ValidProfile_ReturnsFormattedInfo()
	{

		var profile = new Profile(Guid.NewGuid(), "user123", "pass", "bib", "bob", 2000);

		var result = profile.GetInfo();

		Assert.Contains("bib", result);
		Assert.Contains("bob", result);
		Assert.Contains("возраст", result);
	}

	[Fact]
	public void Constructor_WithValidParameters_CreatesProfile()
	{
		var id = Guid.NewGuid();
		var login = "testuser";
		var password = "password123";
		var firstName = "First";
		var lastName = "Last";
		var birthYear = 2000;

		var profile = new Profile(id, login, password, firstName, lastName, birthYear);

		Assert.Equal(id, profile.Id);
		Assert.Equal(login, profile.Login);
		Assert.Equal(password, profile.Password);
		Assert.Equal(firstName, profile.FirstName);
		Assert.Equal(lastName, profile.LastName);
		Assert.Equal(birthYear, profile.BirthYear);
	}
}