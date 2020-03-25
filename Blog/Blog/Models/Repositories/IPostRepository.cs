using Blog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models.Repositories
{

	public interface IPostRepository : IRepository<Post>
	{
		IEnumerable<Post> GetByTitle(string title);
		Post GetFirstByTitle(string title);
		IEnumerable<Post> GetByTag(string tag);
		Post GetFirstByTag(string tag);
		IEnumerable<Post> SortByDate();
	}
}
