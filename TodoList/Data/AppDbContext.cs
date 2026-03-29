using Microsoft.EntityFrameworkCore;
using TodoList.Models;

namespace TodoList.Data;

public class AppDbContext : DbContext
{
	public DbSet<TodoItem> Todos => Set<TodoItem>();
	public DbSet<Profile> Profiles => Set<Profile>();


	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlite("Data Source=todos.db");
	}


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<TodoItem>()
			.HasOne(t => t.Profile)
			.WithMany(p => p.Todos)
			.HasForeignKey(t => t.ProfileId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<Profile>()
			.Property(p => p.Login)
			.IsRequired()
			.HasMaxLength(100);

		modelBuilder.Entity<Profile>()
			.Property(p => p.Password)
			.IsRequired()
			.HasMaxLength(100);

		modelBuilder.Entity<Profile>()
			.Property(p => p.FirstName)
			.IsRequired()
			.HasMaxLength(50);

		modelBuilder.Entity<Profile>()
			.Property(p => p.LastName)
			.IsRequired()
			.HasMaxLength(50);

		modelBuilder.Entity<Profile>()
			.Property(p => p.BirthYear)
			.IsRequired();

		modelBuilder.Entity<TodoItem>()
			.Property(t => t.Text)
			.IsRequired();

		modelBuilder.Entity<TodoItem>()
			.Property(t => t.Status)
			.IsRequired();

		modelBuilder.Entity<TodoItem>()
			.Property(t => t.LastUpdate)
			.IsRequired();
	}
}
