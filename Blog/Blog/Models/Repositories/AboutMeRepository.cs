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

		public AboutMe GetFirst()
		{
			return context.AboutMe.Include(x => x.Contacts).Include(x => x.Websites).FirstOrDefault();
		}

		public IEnumerable<AboutMe> Get()
		{
			return context.AboutMe;
		}

		public AboutMe Get(int id)
		{
			return context.AboutMe.Include(x => x.Contacts).Include(x => x.Websites).FirstOrDefault(x => x.Id == id);
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
				.Include(x => x.Websites)
				.FirstOrDefault(x => x.Id == id) ??
					throw new ArgumentOutOfRangeException("Can't find and update item with id: " + id);

			if (aboutMe.Contacts != null)
				UpdateContacts(aboutMe, oldAboutMe);

			if (aboutMe.Websites != null)
				UpdateWebsites(aboutMe, oldAboutMe);

			if (image != null)
			{
				oldAboutMe.ImagePath = await fileManager.SaveOrCreateImage(
					oldAboutMe.ImagePath,
					aboutMe.ImagePath,
					image);
			}

			oldAboutMe.Title = aboutMe.Title;
			oldAboutMe.Excerpt = aboutMe.Excerpt;
			oldAboutMe.Description = aboutMe.Description;

			oldAboutMe.Updated = DateTime.Now;
		}

		private void UpdateContacts(AboutMe aboutMe, AboutMe oldAboutMe)
		{
			if (oldAboutMe.Contacts == null)
				context.Contacts.Add(aboutMe.Contacts);
			else
			{
				oldAboutMe.Contacts.Phone = aboutMe.Contacts.Phone;
				oldAboutMe.Contacts.Email = aboutMe.Contacts.Email;
				oldAboutMe.Contacts.Country = aboutMe.Contacts.Country;
				oldAboutMe.Contacts.City = aboutMe.Contacts.City;
				oldAboutMe.Contacts.PostAddress = aboutMe.Contacts.PostAddress;
			}
		}

		private void UpdateWebsites(AboutMe aboutMe, AboutMe oldAboutMe)
		{
			if (oldAboutMe.Websites == null)
				context.Websites.Add(aboutMe.Websites);
			else
			{
				oldAboutMe.Websites.Website = aboutMe.Websites.Website;
				oldAboutMe.Websites.Github = aboutMe.Websites.Github;
				oldAboutMe.Websites.LinkedIn = aboutMe.Websites.LinkedIn;
				oldAboutMe.Websites.Facebook = aboutMe.Websites.Facebook;
				oldAboutMe.Websites.Instagram = aboutMe.Websites.Instagram;
				oldAboutMe.Websites.Twitter = aboutMe.Websites.Twitter;
				oldAboutMe.Websites.Youtube = aboutMe.Websites.Youtube;
				oldAboutMe.Websites.Vkontakte = aboutMe.Websites.Vkontakte;
			}
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
