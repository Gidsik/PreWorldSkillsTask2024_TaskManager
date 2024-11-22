using Microsoft.EntityFrameworkCore;
using TaskManager.Models.Database.Entities;

namespace TaskManager.Models.Database;

public class AppDbContext : DbContext
{
	public DbSet<WorkTask> WorkTasks { get; set; } = null!;
	public DbSet<WorkTaskCategory> WorkTaskCategories { get; set; } = null!;
	public DbSet<WorkTaskStatus> WorkTaskStatuses { get; set; } = null!;
	public DbSet<User> Users { get; set; } = null!;
	public DbSet<Role> Roles { get; set; } = null!;

	public AppDbContext(DbContextOptions options) : base(options)
	{
		bool needToDelete = false;
		if (needToDelete)
		{
			Database.EnsureDeleted();
		}
		Database.EnsureCreated();
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		//List<Role> roles = [
		//	new () { Id = 1, Name = "Admin" },
		//	new () { Id = 2, Name = "User" }
		//];
		//modelBuilder.Entity<Role>().HasData(roles);

		//List<User> users = [
		//	new(){
		//		Id = 1,
		//		Login = "Admin",
		//		Password = "Admin123",
		//		Role = roles[0]
		//	},
		//	new(){
		//		Id = 2,
		//		Login = "User1",
		//		Password = "User123",
		//		Role = roles[1]
		//	},
		//	new(){
		//		Id = 3,
		//		Login = "User2",
		//		Password = "User123",
		//		Role = roles[1]
		//	}
		//];
		//modelBuilder.Entity<User>().HasData(users);

		//List<WorkTaskStatus> wtStatuses = [
		//	new() { Id = 1, Name = "NotStarted" },
		//	new() { Id = 2, Name = "InProgress" },
		//	new() { Id = 3, Name = "Finished" },
		//];
		//modelBuilder.Entity<WorkTaskStatus>().HasData(wtStatuses);

		//List<WorkTaskCategory> wtCategories = [
		//	new() { Id = 1, Name = "Normal" },
		//	new() { Id = 2, Name = "Critical" },
		//	new() { Id = 3, Name = "Blocker" },
		//];
		//modelBuilder.Entity<WorkTaskCategory>().HasData(wtCategories);

		//List<WorkTask> tasks = [
		//	new() { 
		//		Id = 1, Name = "someTask1", Description = "someDescription", 
		//		Status = wtStatuses[Random.Shared.Next(3)], Category = wtCategories[Random.Shared.Next(3)],
		//		Creator = users[0], Assignee = users[1],
		//		Created = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(10,21)),
		//		Finished = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(1,11))
		//	},
		//	new() {
		//		Id = 1, Name = "someTask2", Description = "someDescription",
		//		Status = wtStatuses[Random.Shared.Next(3)], Category = wtCategories[Random.Shared.Next(3)],
		//		Creator = users[0], Assignee = users[1],
		//		Created = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(10,21)),
		//		Finished = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(1,11))
		//	},
		//	new() {
		//		Id = 1, Name = "someTask3", Description = "someDescription",
		//		Status = wtStatuses[Random.Shared.Next(3)], Category = wtCategories[Random.Shared.Next(3)],
		//		Creator = users[0], Assignee = users[1],
		//		Created = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(10,21)),
		//		Finished = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(1,11))
		//	},
		//	new() {
		//		Id = 1, Name = "someTask4", Description = "someDescription",
		//		Status = wtStatuses[Random.Shared.Next(3)], Category = wtCategories[Random.Shared.Next(3)],
		//		Creator = users[0], Assignee = users[1],
		//		Created = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(10,21)),
		//		Finished = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(1,11))
		//	},
		//	new() {
		//		Id = 1, Name = "someTask5", Description = "someDescription",
		//		Status = wtStatuses[Random.Shared.Next(3)], Category = wtCategories[Random.Shared.Next(3)],
		//		Creator = users[0], Assignee = users[1],
		//		Created = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(10,21)),
		//		Finished = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(1,11))
		//	},
		//	new() {
		//		Id = 1, Name = "someTask6", Description = "someDescription",
		//		Status = wtStatuses[Random.Shared.Next(3)], Category = wtCategories[Random.Shared.Next(3)],
		//		Creator = users[0], Assignee = users[1],
		//		Created = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(10,21)),
		//		Finished = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(1,11))
		//	},
		//	new() {
		//		Id = 1, Name = "someTask7", Description = "someDescription",
		//		Status = wtStatuses[Random.Shared.Next(3)], Category = wtCategories[Random.Shared.Next(3)],
		//		Creator = users[0], Assignee = users[1],
		//		Created = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(10,21)),
		//		Finished = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(1,11))
		//	},
		//	new() {
		//		Id = 1, Name = "someTask8", Description = "someDescription",
		//		Status = wtStatuses[Random.Shared.Next(3)], Category = wtCategories[Random.Shared.Next(3)],
		//		Creator = users[0], Assignee = users[1],
		//		Created = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(10,21)),
		//		Finished = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(1,11))
		//	},
		//	new() {
		//		Id = 1, Name = "someTask9", Description = "someDescription",
		//		Status = wtStatuses[Random.Shared.Next(3)], Category = wtCategories[Random.Shared.Next(3)],
		//		Creator = users[0], Assignee = users[1],
		//		Created = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(10,21)),
		//		Finished = null
		//	},
		//	new() {
		//		Id = 1, Name = "someTask10", Description = "someDescription",
		//		Status = wtStatuses[Random.Shared.Next(3)], Category = wtCategories[Random.Shared.Next(3)],
		//		Creator = users[0], Assignee = users[1],
		//		Created = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(10,21)),
		//		Finished = null
		//	},
		//	new() {
		//		Id = 1, Name = "someTask1_2", Description = "someDescription",
		//		Status = wtStatuses[Random.Shared.Next(3)], Category = wtCategories[Random.Shared.Next(3)],
		//		Creator = users[0], Assignee = users[2],
		//		Created = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(10,21)),
		//		Finished = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(1,11))
		//	},
		//	new() {
		//		Id = 1, Name = "someTask2_2", Description = "someDescription",
		//		Status = wtStatuses[Random.Shared.Next(3)], Category = wtCategories[Random.Shared.Next(3)],
		//		Creator = users[0], Assignee = users[2],
		//		Created = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(10,21)),
		//		Finished = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(1,11))
		//	},
		//	new() {
		//		Id = 1, Name = "someTask3_2", Description = "someDescription",
		//		Status = wtStatuses[Random.Shared.Next(3)], Category = wtCategories[Random.Shared.Next(3)],
		//		Creator = users[0], Assignee = users[2],
		//		Created = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(10,21)),
		//		Finished = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(1,11))
		//	},
		//	new() {
		//		Id = 1, Name = "someTask4_2", Description = "someDescription",
		//		Status = wtStatuses[Random.Shared.Next(3)], Category = wtCategories[Random.Shared.Next(3)],
		//		Creator = users[0], Assignee = users[2],
		//		Created = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(10,21)),
		//		Finished = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(1,11))
		//	},
		//	new() {
		//		Id = 1, Name = "someTask5_2", Description = "someDescription",
		//		Status = wtStatuses[Random.Shared.Next(3)], Category = wtCategories[Random.Shared.Next(3)],
		//		Creator = users[0], Assignee = users[2],
		//		Created = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(10,21)),
		//		Finished = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(1,11))
		//	},
		//	new() {
		//		Id = 1, Name = "someTask6_2", Description = "someDescription",
		//		Status = wtStatuses[Random.Shared.Next(3)], Category = wtCategories[Random.Shared.Next(3)],
		//		Creator = users[0], Assignee = users[2],
		//		Created = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(10,21)),
		//		Finished = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(1,11))
		//	},
		//	new() {
		//		Id = 1, Name = "someTask7_2", Description = "someDescription",
		//		Status = wtStatuses[Random.Shared.Next(3)], Category = wtCategories[Random.Shared.Next(3)],
		//		Creator = users[0], Assignee = users[2],
		//		Created = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(10,21)),
		//		Finished = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(1,11))
		//	},
		//	new() {
		//		Id = 1, Name = "someTask8_2", Description = "someDescription",
		//		Status = wtStatuses[Random.Shared.Next(3)], Category = wtCategories[Random.Shared.Next(3)],
		//		Creator = users[0], Assignee = users[2],
		//		Created = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(10,21)),
		//		Finished = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(1,11))
		//	},
		//	new() {
		//		Id = 1, Name = "someTask9_2", Description = "someDescription",
		//		Status = wtStatuses[Random.Shared.Next(3)], Category = wtCategories[Random.Shared.Next(3)],
		//		Creator = users[0], Assignee = users[2],
		//		Created = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(10,21)),
		//		Finished = null
		//	},
		//	new() {
		//		Id = 1, Name = "someTask10_2", Description = "someDescription",
		//		Status = wtStatuses[Random.Shared.Next(3)], Category = wtCategories[Random.Shared.Next(3)],
		//		Creator = users[0], Assignee = users[2],
		//		Created = DateTime.UtcNow - TimeSpan.FromDays(Random.Shared.Next(10,21)),
		//		Finished = null
		//	}
		//];
		//modelBuilder.Entity<WorkTask>().HasData(tasks);
	}
}
