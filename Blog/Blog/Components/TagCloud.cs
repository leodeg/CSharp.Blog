using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Models.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Components
{
	public class TagCloud : ViewComponent
	{
		private readonly ITagRepository tagRepository;

		public TagCloud(ITagRepository tagRepository)
		{
			this.tagRepository = tagRepository;
		}

		public IViewComponentResult Invoke()
		{
			var tags = tagRepository.Get();
			return View(tags);
		}
	}
}