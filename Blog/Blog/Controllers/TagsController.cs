using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Models;
using Blog.Models.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
	public class TagsController : Controller
	{
		private readonly IRepository<Tag> repository;
		private readonly string TagForm = "TagForm";

		public TagsController(IRepository<Tag> repository)
		{
			this.repository = repository;
		}

		public IActionResult Index()
		{
			return View(repository.Get().OrderBy(x => x.Name));
		}

		public IActionResult Create()
		{
			return View(TagForm, new Tag());
		}

		public IActionResult Edit(int id)
		{
			var tag = repository.Get(id);
			if (tag == null)
				return NotFound();
			return View(TagForm, tag);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Save(Tag tag)
		{
			if (!ModelState.IsValid)
				return View(TagForm, tag);

			if (tag.Id == 0)
				repository.Create(tag);
			else repository.Update(tag.Id, tag);

			await repository.SaveAsync();
			return RedirectToAction(nameof(Index));
		}

		public async Task<ActionResult> Remove(int id)
		{
			if (!repository.Delete(id))
				return NotFound();
			await repository.SaveAsync();
			return RedirectToAction(nameof(Index));
		}
	}
}