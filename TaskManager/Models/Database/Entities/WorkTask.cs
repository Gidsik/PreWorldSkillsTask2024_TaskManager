namespace TaskManager.Models.Database.Entities;

public class WorkTask
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public DateTime Created { get; set; }
	public DateTime? Finished { get; set; }
	public User? Assignee { get; set; }
	public User Creator { get; set; } = null!;
	public WorkTaskStatus? Status { get; set; }
	public WorkTaskCategory? Category { get; set; }
}
