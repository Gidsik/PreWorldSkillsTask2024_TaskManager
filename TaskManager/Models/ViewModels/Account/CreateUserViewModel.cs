using TaskManager.Models.Database.Entities;

namespace TaskManager.Models.ViewModels.Account
{
	public class CreateUserViewModel
	{
		public string Login { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;

		public int RoleId { get; set; }
		public IEnumerable<Role> Roles { get; set; } = [];
	}
}
