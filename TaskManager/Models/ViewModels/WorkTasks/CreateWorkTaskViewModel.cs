using TaskManager.Models.Database.Entities;

namespace TaskManager.Models.ViewModels.WorkTasks
{
	public class CreateWorkTaskViewModel
	{
		public string WorkTaskName { get; set; } = string.Empty;
		public string WorkTaskDescription { get; set; } = string.Empty;

		public int? AssigneeId { get; set; }
		public IEnumerable<User> Users { get; set; } = [];

		public int StatusId { get; set; }
		public IEnumerable<WorkTaskStatus> Statuses { get; set; } = [];

		public int CategoryId { get; set; }
		public IEnumerable<WorkTaskCategory> Categories { get; set; } = [];
	}
}
