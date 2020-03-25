using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
	public class Post : Model
	{
		[Required]
		public string Title { get; set; }
		[Required]
		public string Content { get; set; }
		[Required]
		public string Excerpt { get; set; }
		public string Tags { get; set; }

		public string ImagePath { get; set; }

		public int Views { get; set; }
		public bool Public { get; set; } = true;

		public List<string> TagsList
		{
			get
			{
				if (!string.IsNullOrEmpty(Tags))
					return Tags.Split(",").ToList();
				return null;
			}
		}
	}
}
