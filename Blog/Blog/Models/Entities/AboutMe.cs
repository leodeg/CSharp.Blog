using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{

	public class AboutMe : Model
	{
		[ForeignKey(name: "Contacts")]
		public int ContactsId { get; set; }
		public virtual Contacts Contacts { get; set; }

		[ForeignKey(name: "Websites")]
		public int WebsitesId { get; set; }
		public virtual Websites Websites { get; set; }
	}
}
