using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{

	public class Websites : Model
	{
		public string Website { get; set; }
		public string Github { get; set; }
		public string LinkedIn { get; set; }
		public string Facebook { get; set; }
		public string Instagram { get; set; }
		public string Twitter { get; set; }
		public string Youtube { get; set; }
		public string Vkontakte { get; set; }
	}
}
