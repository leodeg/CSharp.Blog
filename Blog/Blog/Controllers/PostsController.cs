using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Blog.Data;
using Blog.Models;
using Blog.Models.Repositories;

namespace Blog.Controllers
{
	public class PostsController : Controller
	{
		private readonly IRepository<Post> repository;
		private readonly string PostForm = "PostForm";

		public PostsController(IRepository<Post> repository)
		{
			this.repository = repository;
		}

		public IActionResult Index()
		{
			return View(repository.Get());
		}

		public IActionResult Details(int? id)
		{
			if (id == null)
				return NotFound();

			var post = repository.Get(id.Value);
			if (post == null)
				return NotFound();

			return View(post);
		}

		public IActionResult Create()
		{
			return View(PostForm, new Post());
		}

		public IActionResult Edit(int? id)
		{
			Post post = repository.Get(id.Value);
			if (post == null)
				return NotFound();

			return View(PostForm, post);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Save(Post post)
		{
			if (!ModelState.IsValid)
				return View(PostForm, post);

			if (post.Id == 0)
				repository.Create(post);
			else repository.Update(post.Id, post);

			await repository.SaveAsync();
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
				return NotFound();

			if (!repository.Delete(id.Value))
				return NotFound();

			if (await repository.SaveAsync())
				return RedirectToAction(nameof(Index));

			return BadRequest();
		}
	}
}
