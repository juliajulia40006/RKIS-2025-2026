using TodoList.Models;
using TodoList.Data;

namespace TodoList.Services;

public class TodoRepository
{
	public List<TodoItem> GetAll(Guid profileId)
	{
		using var context = new AppDbContext();
		return context.Todos
			.Where(t => t.ProfileId == profileId)
			.ToList();
	}

	public void Add(TodoItem item, Guid profileId)
	{
		using var context = new AppDbContext();
		item.ProfileId = profileId;
		if (item.Id != 0)
		{
			var existingItem = context.Todos.Find(item.Id);
			if (existingItem != null)
			{
				context.Entry(existingItem).CurrentValues.SetValues(item);
				context.SaveChanges();
				return;
			}
		}
		context.Todos.Add(item);
		context.SaveChanges();
	}

	public void Update(TodoItem item)
	{
		using var context = new AppDbContext();
		var existingItem = context.Todos.Find(item.Id);
		if (existingItem != null)
		{
			context.Entry(existingItem).CurrentValues.SetValues(item);
		}
		else
		{
			context.Todos.Attach(item);
			context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
		}
		context.SaveChanges();
	}

	public void Delete(int id)
	{
		using var context = new AppDbContext();
		var item = context.Todos.Find(id);
		if (item != null)
		{
			context.Todos.Remove(item);
			context.SaveChanges();
		}
	}
	public void SetStatus(int id, TodoStatus status)
	{
		using var context = new AppDbContext();
		var item = context.Todos.Find(id);
		if (item != null)
		{
			item.SetStatus(status);
			context.Todos.Update(item);
			context.SaveChanges();
		}
	}
}