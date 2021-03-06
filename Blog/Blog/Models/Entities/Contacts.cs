﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
	public class Contacts : Model
	{
		[DataType(DataType.PhoneNumber)]
		public string Phone { get; set; }
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[Required]
		public string Country { get; set; }
		public string City { get; set; }

		[Display(Name = "Post Address")]
		public string PostAddress { get; set; }
	}
}
