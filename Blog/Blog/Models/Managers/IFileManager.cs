using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
	public interface IFileManager
	{
		Task<string> SaveImage(IFormFile image);
		void DeleteImage(string imageName);
		Task<string> SaveOrCreateImage(string oldImagePath, string imagePath, IFormFile image);
	}
}
