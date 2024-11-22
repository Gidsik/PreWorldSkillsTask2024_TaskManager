using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;
using TaskManager.Models.Database;
using TaskManager.Models.Database.Entities;
using TaskManager.Models.ViewModels.Account;
using TaskManager.Services.Interfaces;

namespace TaskManager.Controllers;

public class AccountController : Controller
{
	private readonly IAccountsService _accountsService;
	private readonly AppDbContext _db;

	public AccountController(IAccountsService accountsService, AppDbContext db)
	{
		_accountsService = accountsService;
		_db = db;
	}

	public ActionResult Login()
	{
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> Login(LoginViewModel loginViewModel)
	{
		var loggedInUser = await _accountsService.Login(loginViewModel.Login, loginViewModel.Password);
		if (loggedInUser is null)
		{
			return View(loginViewModel);
		}

		List<Claim> claims = [
			new(ClaimTypes.Name, loggedInUser!.Login),
			new(ClaimTypes.Role, loggedInUser!.Role!.Name)
		];

		var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
		var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

		await HttpContext.SignInAsync(claimsPrincipal);

		return RedirectToAction("Index", "WorkTask");
	}

	[Authorize]
	public async Task<ActionResult> Logout()
	{
		await HttpContext.SignOutAsync();

		return RedirectToAction(controllerName: "Account", actionName: nameof(Login));
	}

	public ActionResult Register()
	{
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> Register(RegisterViewModel registerViewModel)
	{
		if (!ModelState.IsValid || registerViewModel.Password != registerViewModel.ConfirmPassword)
		{
			ModelState.AddModelError("isRegFailed", "Passwords incorrect");
			return View(registerViewModel);
		}

		var newUser = await _accountsService.Register(registerViewModel.Login, registerViewModel.Password);
		if (newUser is null)
		{
			ModelState.AddModelError("isRegFailed", "Login or Email already taken");
			return View(registerViewModel);
		}

		return RedirectToAction(nameof(Login));
	}

	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Index(string? searchQuery)
	{
		IQueryable<User> users = _db.Users.Include(u => u.Role);

		if (!string.IsNullOrEmpty(searchQuery))
		{
			ViewBag.SearchQuery = searchQuery;
			users = users.Where(t => t.Login.Contains(searchQuery));
		}

		return View(await users.ToListAsync());
	}

	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Create()
	{
		var vm = new CreateUserViewModel()
		{
			Roles = await _db.Roles.ToListAsync()
		};
		return View(vm);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Create(string login, string password, int roleId)
	{
		var newUser = await _accountsService.Register(login, password);
		if (newUser is null)
		{
			var vm = new CreateUserViewModel()
			{
				Login = login,
				Password = password,
				RoleId = roleId,
				Roles = await _db.Roles.ToListAsync()
			};
			return View(vm);
		}

		var newRole = _db.Roles.Find(roleId);
		if (newRole is not null)
		{
			newUser.Role = newRole;
			_db.Update(newUser);
			_db.SaveChanges();
		}

		return RedirectToAction(nameof(Index));
	}

	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Edit(int id)
	{
		var user = _db.Users.Include(u => u.Role).SingleOrDefault(u => u.Id == id);
		if (user is null) return RedirectToAction(nameof(Index));

		var vm = new EditUserViewModel()
		{
			Id = user.Id,
			Login = user.Login,
			RoleId = user.Role.Id,
			Roles = await _db.Roles.ToListAsync()
		};

		return View(vm);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Edit(int id, string login, string password, int roleId)
	{
		var user = _db.Users.Include(u => u.Role).SingleOrDefault(u => u.Id == id);
		if (user is null) return RedirectToAction(nameof(Index));

		var newRole = _db.Roles.Find(roleId);
		if (newRole is null)
		{
			var vm = new EditUserViewModel()
			{
				Id = user.Id,
				Login = login,
				RoleId = user.Role.Id,
				Roles = await _db.Roles.ToListAsync()
			};
			return View(vm);
		}

		user.Login = login;
		user.Role = newRole;
		if (!string.IsNullOrEmpty(password))
		{
			user.Password = password;
		}

		_db.Update(user);
		_db.SaveChanges();

		return RedirectToAction(nameof(Index));
	}

	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Delete(int id)
	{
		var user = _db.Users.Find(id);
		if (user is null) return RedirectToAction(nameof(Index));

		_db.Remove(user);
		_db.SaveChanges();

		return RedirectToAction(nameof(Index));
	}
}
