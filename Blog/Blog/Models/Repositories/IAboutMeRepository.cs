using Blog.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models.Repositories
{

	public interface IAboutMeRepository : IRepository<AboutMe>
	{
		AboutMe GetFirst();
		Task Create(AboutMe aboutMe, IFormFile image);
		Task Update(int id, AboutMe aboutMe, IFormFile image);
	}
}
