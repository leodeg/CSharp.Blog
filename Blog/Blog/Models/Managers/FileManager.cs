using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
	public class FileManager : IFileManager
	{
		private readonly string RootImagePath = "Path:Images";
		private string imagePath;

		public FileManager(IConfiguration configuration)
		{
			imagePath = configuration[RootImagePath];
		}

		public async Task<string> SaveImage(IFormFile image)
		{
			string savePath = Path.Combine(imagePath);
			if (!Directory.Exists(savePath))
				Directory.CreateDirectory(savePath);

			string uniqueName = Guid.NewGuid().ToString();
			string imageType = image.FileName.Substring(image.FileName.LastIndexOf("."));
			string fileName = $"img_{uniqueName}{imageType}";

			using (FileStream fileStream = new FileStream(Path.Combine(savePath, fileName), FileMode.Create))
			{
				await image.CopyToAsync(fileStream);
			}

			return fileName;
		}

		public void DeleteImage(string imageName)
		{
			string deletePath = Path.Combine(imagePath);
			if (!Directory.Exists(deletePath))
				throw new FileNotFoundException();

			try
			{
				string filePath = Path.Combine(deletePath, imageName);
				File.Delete(filePath);
			}
			catch (DirectoryNotFoundException ex)
			{
				Trace.WriteLine(ex.Message);
				throw;
			}
		}
	}
}
