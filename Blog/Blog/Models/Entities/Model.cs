using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
	public class Model
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public DateTime Created { get; set; } = DateTime.Now;
		[Required]
		public DateTime Updated { get; set; } = DateTime.Now;
	}
}
