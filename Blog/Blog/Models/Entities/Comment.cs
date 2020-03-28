using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
	public class Comment : Model
	{
		[Required]
		[ForeignKey(nameof(Post))]
		public int PostId { get; set; }
		public virtual Post Post { get; set; }

		[Required]
		[Display (Name = "Name")]
		public string UserName { get; set; }
		[Required]
		public string Message { get; set; }
	}
}
