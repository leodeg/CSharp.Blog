using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{

	public class Contacts : Model
	{
		public string Phone { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		public string Country { get; set; }
		public string City { get; set; }
		public string PostAddress { get; set; }
	}
}
