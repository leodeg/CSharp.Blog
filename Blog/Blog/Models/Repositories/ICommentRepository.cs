using Blog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models.Repositories
{

	public interface ICommentRepository : IRepository<Comment>
	{
		IEnumerable<Comment> GetByPost(int id);
	}
}
