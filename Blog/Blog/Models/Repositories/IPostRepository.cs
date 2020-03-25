using Blog.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models.Repositories
{

	public interface IPostRepository : IRepository<Post>
	{
		Task Create(Post post, IFormFile image);
		Task Update(int id, Post post, IFormFile image);
		IEnumerable<Post> GetByTitle(string title);
		Post GetFirstByTitle(string title);
		IEnumerable<Post> GetByTag(string tag);
		Post GetFirstByTag(string tag);
		IEnumerable<Post> SortByDate();
	}
}
