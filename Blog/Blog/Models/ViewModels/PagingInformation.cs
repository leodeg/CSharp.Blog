using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models.ViewModels
{
	public class PagingInformation
	{
		public int TotalItems { get; set; }
		public int ItemsPerPage { get; set; }
		public int CurrentPage { get; set; }

		public int TotalPages => (int) Math.Ceiling((decimal) TotalItems / ItemsPerPage);
		public bool HasNextPage => CurrentPage < TotalPages;
		public bool HasPreviousPage => CurrentPage > 1;

	}
}
