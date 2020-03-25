using Blog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models.Repositories
{

	public interface ITagRepository : IRepository<Tag>
	{
		void Create(string tag);
		Tag Get(string tag);
		bool Delete(string tag);
	}
}
