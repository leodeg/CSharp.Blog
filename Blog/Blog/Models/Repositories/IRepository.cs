using Blog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models.Repositories
{
	interface IRepository<T>
	{
		IEnumerable<T> Get();
		T Get(int id);
		void Create(T entity);
		void Update(int id, T entity);
		bool Delete(int id);
	}
}
