using Microsoft.EntityFrameworkCore;
using TodoList.Models;

namespace TodoList.Data;

public class AppDbContext : DbContext
{
	public DbSet<TodoItem> Todos => Set<TodoItem>();
	public DbSet<Profile> Profiles => Set<Profile>();


	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "todolist.db");
		optionsBuilder.UseSqlite($"Data Source={dbPath}");
	}


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Profile>(entity =>
		{
			entity.HasKey(p => p.Id);
			entity.HasIndex(p => p.Login).IsUnique();
			entity.Property(p => p.Login).IsRequired();
			entity.Property(p => p.Password).IsRequired();
			entity.Property(p => p.FirstName).IsRequired();
			entity.Property(p => p.LastName).IsRequired();

			entity.HasMany(p => p.Todos)
				  .WithOne(t => t.Profile)
				  .HasForeignKey(t => t.ProfileId)
				  .OnDelete(DeleteBehavior.Cascade);
		});

		modelBuilder.Entity<TodoItem>(entity =>
		{
			entity.HasKey(t => t.Id);
			entity.Property(t => t.Id).ValueGeneratedOnAdd();
			entity.Property(t => t.Text).IsRequired();
			entity.Property(t => t.Status).IsRequired();
			entity.Property(t => t.LastUpdate).IsRequired();

			entity.Property(t => t.ProfileId).IsRequired();

			entity.HasOne(t => t.Profile)
				  .WithMany(p => p.Todos)
				  .HasForeignKey(t => t.ProfileId)
				  .OnDelete(DeleteBehavior.Cascade);
		});
	}
}
