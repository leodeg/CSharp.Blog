using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models.ViewModels
{
	public class BlogViewModel
	{
		public IEnumerable<Post> Posts { get; set; }
		public IEnumerable<Tag> Tags { get; set; }
		public PagingInformation PagingInformation { get; set; }
	}
}
