using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models.ViewModels
{

	public class CommentViewModel
	{
		public int PostId { get; set; }
		public Comment Comment { get; set; }
		public string ReturnUrl { get; set; }
	}
}
