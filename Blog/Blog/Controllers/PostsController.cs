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
using Blog.Models.ViewModels;

namespace Blog.Controllers
{
	public class PostsController : Controller
	{
		private readonly IPostRepository postsRepository;
		private readonly ITagRepository tagsRepository;
		private readonly IFileManager fileManager;

		private readonly int ItemsPerPage = 50;
		private readonly string PostForm = "PostForm";

		public PostsController(IPostRepository postsRepository, ITagRepository tagsRepository, IFileManager fileManager)
		{
			this.postsRepository = postsRepository;
			this.tagsRepository = tagsRepository;
			this.fileManager = fileManager;
		}

		public IActionResult Index(string tag = "", string title = "", int page = 1)
		{
			var posts = GetPosts(tag, title);
			int totalPosts = posts.Count();

			if (totalPosts > ItemsPerPage)
				posts = posts.Skip((page - 1) * ItemsPerPage).Take(ItemsPerPage);

			PostsViewModel homeViewModel = new PostsViewModel()
			{
				Posts = posts,
				PagingInformation = new PagingInformation()
				{
					CurrentPage = page,
					ItemsPerPage = ItemsPerPage,
					TotalItems = totalPosts
				}
			};

			return View(homeViewModel);
		}

		private IEnumerable<Post> GetPosts(string tag = "", string title = "")
		{
			if (!string.IsNullOrEmpty(tag))
				return postsRepository.GetByTag(tag);

			if (!string.IsNullOrEmpty(title))
				return postsRepository.GetByTitle(title);

			return postsRepository.Get();
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

			await postsRepository.SaveAsync();
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
				await postsRepository.Create(post, image);
			else
				await postsRepository.Update(post.Id, post, image);
		}

		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
				return NotFound();

			if (!postsRepository.Delete(id.Value))
				return NotFound();

			if (await postsRepository.SaveAsync())
				return RedirectToAction(nameof(Index));

			return BadRequest();
		}
	}
}
