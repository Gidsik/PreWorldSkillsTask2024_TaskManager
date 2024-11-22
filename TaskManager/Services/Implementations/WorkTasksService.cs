using Microsoft.EntityFrameworkCore;
using TaskManager.Models.Database;
using TaskManager.Models.Database.Entities;
using TaskManager.Services.Interfaces;

namespace TaskManager.Services.Implementations;

public class WorkTasksService : IWorkTasksService
{
	private readonly AppDbContext _db;

	public WorkTasksService(AppDbContext db)
	{
		_db = db;
	}

	public async Task<WorkTask?> GetTaskById(int id)
	{
		return await GetAllTasks().Where(t => t.Id == id).SingleOrDefaultAsync();
	}

	public IQueryable<WorkTask> GetAllTasks() 
		=> _db.WorkTasks
		.Include(t => t.Creator)
		.Include(t => t.Assignee)
		.Include(t => t.Category)
		.Include(t => t.Status);

	public IQueryable<WorkTask> GetTasksOfUser(string userName)
	{
		var tasks = GetAllTasks()
			.Where(t => t.Assignee != null && t.Assignee.Login == userName);

		return tasks;
	}
}
