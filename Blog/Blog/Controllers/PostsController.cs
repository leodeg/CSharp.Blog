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
using Microsoft.AspNetCore.Http;

namespace Blog.Controllers
{
	public class PostsController : Controller
	{
		private readonly IPostRepository postsRepository;
		private readonly ITagRepository tagsRepository;
		private readonly IFileManager fileManager;

		private readonly string PostForm = "PostForm";

		public PostsController(IPostRepository postsRepository, ITagRepository tagsRepository, IFileManager fileManager)
		{
			this.postsRepository = postsRepository;
			this.tagsRepository = tagsRepository;
			this.fileManager = fileManager;
		}

		public IActionResult Index(string tag = "", string title = "")
		{
			if (!string.IsNullOrEmpty(tag))
				return View(postsRepository.GetByTag(tag));

			if (!string.IsNullOrEmpty(title))
				return View(postsRepository.GetByTitle(title));

			return View(postsRepository.Get());
		}

		public IActionResult Details(int? id)
		{
			if (id == null)
				return NotFound();

			var post = postsRepository.Get(id.Value);
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
			Post post = postsRepository.Get(id.Value);
			if (post == null)
				return NotFound();

			return View(PostForm, post);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Save(IFormFile image, Post post)
		{
			if (!ModelState.IsValid)
				return View(PostForm, post);

			SaveTags(post);
			await SavePost(image, post);

			return RedirectToAction(nameof(Index));
		}

		private void SaveTags(Post post)
		{
			if (!string.IsNullOrEmpty(post.Tags))
			{
				foreach (var tag in post.TagsList)
				{
					tagsRepository.Create(tag);
				}
			}
		}

		private async Task SavePost(IFormFile image, Post post)
		{
			if (post.Id == 0)
			{
				if (image != null)
					post.ImagePath = await fileManager.SaveImage(image);
				postsRepository.Create(post);
			}
			else
			{
				if (image != null)
				{
					Post oldPost = postsRepository.Get(post.Id);
					if (string.IsNullOrWhiteSpace(oldPost.ImagePath))
					{
						post.ImagePath = await fileManager.SaveImage(image);
					}
					else
					{
						if (oldPost.ImagePath != post.ImagePath)
						{
							fileManager.DeleteImage(oldPost.ImagePath);
							post.ImagePath = await fileManager.SaveImage(image);
						}
					}
				}

				postsRepository.Update(post.Id, post);
			}

			await postsRepository.SaveAsync();
		}

		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
				return NotFound();

			var post = postsRepository.Get(id.Value);
			if (!string.IsNullOrEmpty(post.ImagePath))
				fileManager.DeleteImage(post.ImagePath);

			if (!postsRepository.Delete(id.Value))
				return NotFound();

			if (await postsRepository.SaveAsync())
				return RedirectToAction(nameof(Index));

			return BadRequest();
		}
	}
}
