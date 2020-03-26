using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models.ViewModels
{
	public class PostsViewModel
	{
		public IEnumerable<Post> Posts { get; set; }
		public PagingInformation PagingInformation { get; set; }
	}
}
