using Blog.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models.Repositories
{
	public class AboutMeRepository : IAboutMeRepository
	{
		private ApplicationDbContext context;
		private readonly IFileManager fileManager;

		public AboutMeRepository(ApplicationDbContext context, IFileManager fileManager)
		{
			this.context = context;
			this.fileManager = fileManager;
		}

		public IEnumerable<AboutMe> Get()
		{
			throw new NotImplementedException();
		}

		public AboutMe Get(int id)
		{
			throw new NotImplementedException();
		}

		public async void Create(AboutMe entity)
		{
			await Create(entity, null);
		}

		public async Task Create(AboutMe aboutMe, IFormFile image)
		{
			if (image != null)
				aboutMe.ImagePath = await fileManager.SaveImage(image);

			if (aboutMe.Contacts != null)
				context.Contacts.Add(aboutMe.Contacts);

			if (aboutMe.Websites != null)
				context.Websites.Add(aboutMe.Websites);

			context.AboutMe.Add(aboutMe);
		}

		public async void Update(int id, AboutMe entity)
		{
			await Update(id, entity, null);
		}

		public async Task Update(int id, AboutMe aboutMe, IFormFile image)
		{
			if (aboutMe == null)
				throw new ArgumentNullException();

			AboutMe oldAboutMe = context.AboutMe
				.Include(x => x.Contacts)
				.Include(x => x.Contacts)
				.FirstOrDefault(x => x.Id == id) ??
					throw new ArgumentOutOfRangeException("Can't find and update item with id: " + id);

			if (aboutMe.Contacts != null)
			{
				oldAboutMe.Contacts = aboutMe.Contacts;
				oldAboutMe.Contacts.Id = oldAboutMe.ContactsId;
			}

			if (aboutMe.Websites != null)
			{
				oldAboutMe.Websites = aboutMe.Websites;
				oldAboutMe.Websites.Id = oldAboutMe.WebsitesId;
			}

			if (image != null)
			{
				oldAboutMe.ImagePath = await fileManager.SaveOrCreateImage(
					oldAboutMe.ImagePath,
					aboutMe.ImagePath,
					image);
			}

			oldAboutMe.Updated = DateTime.Now;
		}

		public bool Delete(int id)
		{
			AboutMe aboutMe = context.AboutMe
				.Include(x => x.Contacts)
				.Include(x => x.Contacts)
				.FirstOrDefault(x => x.Id == id);

			if (aboutMe != null)
			{
				if (!string.IsNullOrEmpty(aboutMe.ImagePath))
					fileManager.DeleteImage(aboutMe.ImagePath);

				if (aboutMe.Contacts != null)
					context.Contacts.Remove(aboutMe.Contacts);

				context.AboutMe.Remove(aboutMe);
				return true;
			}
			return false;
		}

		public async Task<bool> SaveAsync()
		{
			if (await context.SaveChangesAsync() > 0)
				return true;
			return false;
		}
	}
}
