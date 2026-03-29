using TodoList.Models;
using TodoList.Data;

namespace TodoList.Services;

public class ProfileRepository
{
	public List<Profile> GetAll()
	{
		using var context = new AppDbContext();
		return context.Profiles.ToList();
	}

	public Profile? GetById(Guid id)
	{
		using var context = new AppDbContext();
		return context.Profiles.Find(id);
	}

	public Profile? GetByLogin(string login)
	{
		using var context = new AppDbContext();
		return context.Profiles.FirstOrDefault(p => p.Login == login);
	}

	public void Add(Profile profile)
	{
		using var context = new AppDbContext();
		context.Profiles.Add(profile);
		context.SaveChanges();
	}

	public void Update(Profile profile)
	{
		using var context = new AppDbContext();
		context.Profiles.Update(profile);
		context.SaveChanges();
	}
	public void Delete(Guid id)
	{
		using var context = new AppDbContext();
		var profile = context.Profiles.Find(id);
		if (profile != null)
		{
			context.Profiles.Remove(profile);
			context.SaveChanges();
		}
	}
}