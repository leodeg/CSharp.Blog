using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;

namespace Blog.Controllers
{
	[Authorize(Roles = Roles.Administrator)]
	public class UsersController : Controller
	{
		RoleManager<IdentityRole> _roleManager;
		UserManager<IdentityUser> _userManager;
		SignInManager<IdentityUser> _signInManager;

		public UsersController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
		{
			_roleManager = roleManager;
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public IActionResult Index()
		{
			return View(_userManager.Users.OrderBy(c => c.Email).ToList());
		}

		public IActionResult RolesList()
		{
			return View(_roleManager.Roles.OrderBy(c => c.Name));
		}

		//[Authorize(Roles = Roles.Administrator)]
		public IActionResult Create() => View();

		//[Authorize(Roles = Roles.Administrator)]
		[HttpPost]
		public async Task<IActionResult> Create(string name)
		{
			if (!string.IsNullOrEmpty(name))
			{
				IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
				if (result.Succeeded)
				{
					return RedirectToAction(nameof(Index));
				}
				else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
			}
			return View(name);
		}

		//[Authorize(Roles = Roles.Administrator)]
		[HttpPost]
		public async Task<IActionResult> Delete(string id)
		{
			IdentityRole role = await _roleManager.FindByIdAsync(id);
			if (role != null)
			{
				IdentityResult result = await _roleManager.DeleteAsync(role);
			}
			return RedirectToAction(nameof(Index));
		}

		//[Authorize(Roles = Roles.Administrator)]
		public async Task<IActionResult> Edit(string userId)
		{
			IdentityUser user = await _userManager.FindByIdAsync(userId);
			if (user != null)
			{
				var userRoles = await _userManager.GetRolesAsync(user);
				var allRoles = _roleManager.Roles.ToList();
				ChangeRoleViewModel model = new ChangeRoleViewModel
				{
					UserId = user.Id,
					UserEmail = user.Email,
					UserRoles = userRoles,
					AllRoles = allRoles
				};
				return View(model);
			}

			return NotFound();
		}

		//[Authorize(Roles = Roles.Administrator)]
		[HttpPost]
		public async Task<IActionResult> Edit(string userId, List<string> roles)
		{
			IdentityUser user = await _userManager.FindByIdAsync(userId);
			if (user != null)
			{
				var userRoles = await _userManager.GetRolesAsync(user);
				var allRoles = _roleManager.Roles.ToList();

				// Exclude added roles
				var addedRoles = roles.Except(userRoles);
				var removedRoles = userRoles.Except(roles);

				await _userManager.AddToRolesAsync(user, addedRoles);
				await _userManager.RemoveFromRolesAsync(user, removedRoles);

				return RedirectToAction(nameof(Index));
			}

			return NotFound();
		}
	}
}