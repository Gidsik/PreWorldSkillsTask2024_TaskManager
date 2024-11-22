using TaskManager.Models.Database.Entities;

namespace TaskManager.Services.Interfaces
{
	public interface IWorkTasksService
	{
		IQueryable<WorkTask> GetAllTasks();
		Task<WorkTask?> GetTaskById(int id);
		IQueryable<WorkTask> GetTasksOfUser(string userName);
	}
}