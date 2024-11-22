using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models.Database;
using TaskManager.Models.Database.Entities;
using TaskManager.Models.ViewModels.WorkTasks;
using TaskManager.Services.Interfaces;

namespace TaskManager.Controllers;

[Authorize]
public class WorkTaskController : Controller
{
	private readonly ILogger<WorkTaskController> _logger;
	private readonly IWorkTasksService _tasksService;
	private readonly AppDbContext _db;

	public WorkTaskController(ILogger<WorkTaskController> logger, IWorkTasksService tasksService, AppDbContext db)
	{
		_logger = logger;
		_tasksService = tasksService;
		_db = db;
	}

	public async Task<IActionResult> Index(string? searchQuery, string? sortBy, bool sortDesc = false)
	{
		IQueryable<WorkTask> tasks = _tasksService.GetAllTasks();

		if (User.IsInRole("User"))
		{
			tasks = _tasksService.GetTasksOfUser(User.Identity!.Name!);
		}

		if(!string.IsNullOrEmpty(searchQuery))
		{
			ViewBag.SearchQuery = searchQuery;
			tasks = tasks.Where(t => t.Name.Contains(searchQuery));
		}

		ViewBag.SortBy = sortBy;
		ViewBag.SortDesc = sortDesc;
		tasks = sortBy switch
		{
			"created" => sortDesc ? tasks.OrderByDescending(t => t.Created) : tasks.OrderBy(t => t.Created),
			"finished" => sortDesc ? tasks.OrderByDescending(t => t.Finished) : tasks.OrderBy(t => t.Finished),
			_ => tasks,
		};

		return View(await tasks.ToListAsync());
	}

	public async Task<IActionResult> Edit(int id)
	{
		var task = await _tasksService.GetTaskById(id);
		if (task is null) return RedirectToAction(nameof(Index));

		var vm = new EditWorkTaskViewModel()
		{
			WorkTask = task,
			WorkTaskId = task.Id,
			WorkTaskName = task.Name,
			WorkTaskDescription = task.Description,
			CategoryId = task.Category!.Id,
			StatusId = task.Status!.Id,
			AssigneeId = task.Assignee?.Id ?? 0,
			Categories = await _db.WorkTaskCategories.ToListAsync(),
			Statuses = await _db.WorkTaskStatuses.ToListAsync(),
			Users = await _db.Users.ToListAsync(),
		};
		return View(vm);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(int id, string workTaskName, string workTaskDescription, int assigneeId, int statusId, int categoryId)
	{
		var task = await _tasksService.GetTaskById(id);
		if (task is null) return RedirectToAction(nameof(Index));

		if (!User.IsInRole("Admin"))
		{
			var currentUser = await _db.Users.Where(u => u.Login == User.Identity!.Name).SingleOrDefaultAsync();
			if (currentUser is null || task.Assignee!.Id != currentUser.Id)
			{
				return RedirectToAction(nameof(Index));
			}
		}

		var assignee = await _db.Users.FindAsync(assigneeId);
		var status = await _db.WorkTaskStatuses.FindAsync(statusId);
		var category = await _db.WorkTaskCategories.FindAsync(categoryId);

		var isFailed = User.IsInRole("Admin") switch
		{
			true => status is null || category is null,
			false => status is null,
		};

		if (isFailed)
		{
			var vm = new EditWorkTaskViewModel()
			{
				WorkTask = task,
				WorkTaskId = task.Id,
				WorkTaskName = task.Name,
				WorkTaskDescription = task.Description,
				CategoryId = task.Category!.Id,
				StatusId = task.Status!.Id,
				AssigneeId = task.Assignee!.Id,
				Categories = await _db.WorkTaskCategories.ToListAsync(),
				Statuses = await _db.WorkTaskStatuses.ToListAsync(),
				Users = await _db.Users.ToListAsync(),
			};
			return View(vm);
		}

		if (status!.Name == "Finished" && task.Status!.Id != status.Id)
		{
			task.Finished = DateTime.UtcNow;
		}

		if (User.IsInRole("Admin"))
		{
			task.Name = workTaskName;
			task.Description = workTaskDescription;
			task.Status = status;
			task.Category = category;
			task.Assignee = assignee;
		}
		else
		{
			task.Status = status;
		}

		_db.Update(task);
		_db.SaveChanges();

		return RedirectToAction(nameof(Index));
	}


	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Create()
	{
		var vm = new CreateWorkTaskViewModel()
		{
			Categories = await _db.WorkTaskCategories.ToListAsync(),
			Statuses = await _db.WorkTaskStatuses.ToListAsync(),
			Users = await _db.Users.ToListAsync(),
		};
		return View(vm);
	}

	[HttpPost]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Create(string workTaskName, string workTaskDescription, int assigneeId, int statusId, int categoryId)
	{
		var creator = _db.Users.SingleOrDefault(u => u.Login == User.Identity!.Name);
		var assignee = _db.Users.Find(assigneeId);

		var category = _db.WorkTaskCategories.Find(categoryId);
		var status = _db.WorkTaskStatuses.Find(statusId);
		
		if (creator is null || category is null || status is null)
		{
			var vm = new CreateWorkTaskViewModel()
			{
				Categories = await _db.WorkTaskCategories.ToListAsync(),
				Statuses = await _db.WorkTaskStatuses.ToListAsync(),
				Users = await _db.Users.ToListAsync(),
			};
			return View(vm);
		}

		var newTask = new WorkTask()
		{
			Name = workTaskName,
			Description = workTaskDescription,
			Created = DateTime.UtcNow,
			Creator = creator,
			Assignee = assignee,
			Category = category,
			Status = status,
		};

		_db.Add(newTask);
		await _db.SaveChangesAsync();

		return RedirectToAction(nameof(Index));
	}

	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Delete(int id)
	{
		var task = await _tasksService.GetTaskById(id);
		if (task is null) return RedirectToAction(nameof(Index));

		_db.Remove(task);
		_db.SaveChanges();

		return RedirectToAction(nameof(Index));
	}
}
