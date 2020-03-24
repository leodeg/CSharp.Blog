using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models.ViewModels
{
	public class PostViewModel
	{
		public Post Post { get; set; }
		public IFormFile FormFile { get; set; }
	}
}
