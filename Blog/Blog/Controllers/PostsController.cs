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

		public PostsController(IRepository<Post> repository)
		{
			this.repository = repository;
		}

		public IActionResult Index ()
		{
			return View(repository.Get());
		}

		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
				return NotFound();

			var post = repository.Get(id.Value);
			if (post == null)
				return NotFound();

			return View(post);
		}

		public async Task<IActionResult> Edit(Post editPost = null)
		{
			Post post = editPost?? new Post();
			return View(post);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Save([Bind("Title,Content,Excerpt,CoverImagePath,Views,Public,Id,Created,Updated")] Post post)
		{
			if (ModelState.IsValid)
			{
				if (post.Id == 0)
					repository.Create(post);
				else repository.Update(post.Id, post);
				return RedirectToAction(nameof(Index), nameof(HomeController));
			}
			return RedirectToAction(nameof(Edit), post);
		}

		public IActionResult Delete(int? id)
		{
			if (id == null)
				return NotFound();

			repository.Delete(id.Value);
			return RedirectToAction(nameof(Index), nameof(HomeController));
		}
	}
}
