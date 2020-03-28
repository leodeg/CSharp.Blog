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

		public CommentsController(ICommentRepository commentRepository)
		{
			this.commentRepository = commentRepository;
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CommentViewModel commentViewModel)
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