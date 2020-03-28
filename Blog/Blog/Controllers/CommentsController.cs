using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Models;
using Blog.Models.Repositories;
using Blog.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
	public class CommentsController : Controller
	{
		private readonly ICommentRepository commentRepository;
		private readonly int ItemsPerPage = 50;

		public CommentsController(ICommentRepository commentRepository)
		{
			this.commentRepository = commentRepository;
		}

		public IActionResult Index(string userName = "", int page = 1)
		{
			var comments = GetComments(userName);
			int totalPosts = comments.Count();

			if (totalPosts > ItemsPerPage)
				comments = comments.Skip((page - 1) * ItemsPerPage).Take(ItemsPerPage);

			CommentsViewModel commentsViewModel = new CommentsViewModel()
			{
				Comments = comments,
				PagingInformation = new PagingInformation()
				{
					CurrentPage = page,
					ItemsPerPage = ItemsPerPage,
					TotalItems = totalPosts
				}
			};

			return View(commentsViewModel);
		}

		private IEnumerable<Comment> GetComments(string title = "")
		{
			if (!string.IsNullOrEmpty(title))
				return commentRepository.GetByUserName(title);

			return commentRepository.Get();
		}

		public IActionResult Details(int? id)
		{
			if (id == null)
				return NotFound();

			var comment = commentRepository.Get(id.Value);
			if (comment == null)
				return NotFound();

			return View(comment);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CommentPostViewModel commentViewModel)
		{
			if (commentViewModel == null)
				return BadRequest();

			commentViewModel.Comment.PostId = commentViewModel.PostId;
			commentRepository.Create(commentViewModel.Comment);
			await commentRepository.SaveAsync();
			return Redirect(commentViewModel.ReturnUrl);
		}

		public async Task<IActionResult> Delete(int? id, string returnUrl)
		{
			if (id == null)
				return NotFound();

			if (!commentRepository.Delete(id.Value))
				return BadRequest();

			await commentRepository.SaveAsync();
			return Redirect(returnUrl);
		}
	}
}