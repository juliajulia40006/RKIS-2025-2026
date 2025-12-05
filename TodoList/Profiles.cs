
namespace TodoList;
public class Profiles
{
	private List<Profile> profiles = new List<Profile>();

	public void Add(Profile profile)
	{
		profiles.Add(profile);
	}

	public Profile FindByLogin(string login)
	{
		return profiles.FirstOrDefault(p => p.Login.Equals(login, StringComparison.OrdinalIgnoreCase));
	}

	public Profile FindById(Guid id)
	{
		return profiles.FirstOrDefault(p => p.Id == id);
	}

	public IReadOnlyList<Profile> GetAll() => profiles.AsReadOnly();
}
