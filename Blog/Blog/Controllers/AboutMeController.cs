using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Models;
using Blog.Models.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
	public class AboutMeController : Controller
	{
		private readonly IAboutMeRepository repository;

		private readonly string AboutMeForm = "AboutMeForm";

		public AboutMeController(IAboutMeRepository repository)
		{
			this.repository = repository;
		}

		public IActionResult Index()
		{
			return View(repository.GetFirst() ?? new AboutMe());
		}

		public IActionResult Details(int? id)
		{
			if (id == null)
				return NotFound();

			var aboutMe = repository.Get(id.Value);
			if (aboutMe == null)
				return NotFound();

			return View(aboutMe);
		}

		public IActionResult Create()
		{
			return View(AboutMeForm, new AboutMe());
		}

		public IActionResult Edit(int? id)
		{
			AboutMe post = repository.Get(id.Value);
			if (post == null)
				return NotFound();

			return View(AboutMeForm, post);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
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