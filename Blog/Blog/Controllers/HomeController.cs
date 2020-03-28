using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Blog.Models;
using Blog.Models.Repositories;
using Blog.Models.ViewModels;

namespace Blog.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		private readonly ITagRepository tagsRepository;
		private readonly IPostRepository postsRepository;
		private readonly ICommentRepository commentRepository;
		private readonly int ItemsPerPage = 4;

		public HomeController(ILogger<HomeController> logger, ITagRepository tagsRepository, IPostRepository postsRepository, ICommentRepository commentRepository)
		{
			_logger = logger;
			this.tagsRepository = tagsRepository;
			this.postsRepository = postsRepository;
			this.commentRepository = commentRepository;
		}

		public IActionResult Index(string tag = "", string title = "", int page = 1)
		{
			var posts = GetPosts(tag, title);
			int totalPosts = posts.Count();

			if (totalPosts > ItemsPerPage)
				posts = posts.Skip((page - 1) * ItemsPerPage).Take(ItemsPerPage);

			HomeViewModel homeViewModel = new HomeViewModel()
			{
				Posts = posts,
				Tags = tagsRepository.Get(),
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

			Post post = postsRepository.Get(id.Value);
			if (post == null)
				return NotFound();

			PostDetailsViewModel view = new PostDetailsViewModel()
			{
				Post = post,
				Comments = commentRepository.GetByPost(post.Id)
			};

			++post.Views;
			postsRepository.SaveAsync();
			return View(view);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
