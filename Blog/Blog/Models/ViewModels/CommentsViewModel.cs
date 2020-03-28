using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models.ViewModels
{

	public class CommentsViewModel
	{
		public IEnumerable<Comment> Comments { get; set; }
		public PagingInformation PagingInformation { get; set; }
	}
}
