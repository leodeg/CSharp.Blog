using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Models;
using Blog.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
	[Authorize(Roles = Roles.Editor)]
	public class AboutMeController : Controller
	{
		private readonly IAboutMeRepository repository;

		private readonly string AboutMeForm = "AboutMeForm";

		public AboutMeController(IAboutMeRepository repository)
		{
			this.repository = repository;
		}

		[AllowAnonymous]
		public IActionResult Index()
		{
			return View(repository.GetFirst() ?? new AboutMe());
		}

		[Authorize(Roles = Roles.Editor)]
		public IActionResult Details(int? id)
		{
			if (id == null)
				return NotFound();

			var aboutMe = repository.Get(id.Value);
			if (aboutMe == null)
				return NotFound();

			return View(aboutMe);
		}

		[Authorize(Roles = Roles.Editor)]
		public IActionResult Create()
		{
			return View(AboutMeForm, new AboutMe());
		}

		[Authorize(Roles = Roles.Editor)]
		public IActionResult Edit(int? id)
		{
			AboutMe post = repository.Get(id.Value);
			if (post == null)
				return NotFound();

			return View(AboutMeForm, post);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = Roles.Editor)]
		public async Task<IActionResult> Save(IFormFile image, AboutMe aboutMe)
		{
			if (!ModelState.IsValid)
				return View(AboutMeForm, aboutMe);

			if (aboutMe.Id == 0)
				repository.Create(aboutMe);
			else
				await repository.Update(aboutMe.Id, aboutMe, image);

			await repository.SaveAsync();
			return RedirectToAction(nameof(Index));
		}
	}
}