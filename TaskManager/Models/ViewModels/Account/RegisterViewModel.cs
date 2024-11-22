using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models.ViewModels.Account;

public class RegisterViewModel
{
	[Required(ErrorMessage = "Incorrect login given")]
	public string Login { get; set; } = null!;

	[Required(ErrorMessage = "Incorrect password given")]
	[DataType(DataType.Password)]
	public string Password { get; set; } = null!;

	[Required(ErrorMessage = "Passwords are not the same")]
	[DataType(DataType.Password)]
	public string ConfirmPassword { get; set; } = null!;
}
