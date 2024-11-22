using TaskManager.Models.Database.Entities;

namespace TaskManager.Services.Interfaces;

public interface IAccountsService
{
	Task<User?> Login(string login, string password);
	Task<User?> Register(string login, string password);
}