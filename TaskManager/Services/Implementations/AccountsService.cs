using Microsoft.EntityFrameworkCore;
using TaskManager.Models.Database;
using TaskManager.Models.Database.Entities;
using TaskManager.Services.Interfaces;

namespace TaskManager.Services.Implementations;

public class AccountsService : Interfaces.IAccountsService
{
	private readonly AppDbContext _db;

	public AccountsService(AppDbContext db)
	{
		_db = db;
	}

	public async Task<User?> Login(string login, string password)
	{
		return await _db.Users
			.Where(u => u.Login == login && u.Password == password)
			.Include(u => u.Role)
			.SingleOrDefaultAsync();
	}

	public async Task<User?> Register(string login, string password)
	{
		if (_db.Users.Where(u => u.Login == login).Any())
		{
			return null;
		}
		var role = _db.Roles.SingleOrDefault(r => r.Name == "User");
		if (role is null)
		{
			return null;
		}
		var newUser = new User()
		{
			Login = login,
			Password = password,
			Role = role
		};
		_db.Users.Add(newUser);
		await _db.SaveChangesAsync();
		return newUser;
	}
}
