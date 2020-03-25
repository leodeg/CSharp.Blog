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
	public class PostRepository : IPostRepository
	{
		private ApplicationDbContext context;
		private readonly IFileManager fileManager;

		public PostRepository(ApplicationDbContext context, IFileManager fileManager)
		{
			this.context = context;
			this.fileManager = fileManager;
		}

		public IEnumerable<Post> Get()
		{
			return context.Posts;
		}

		public Post Get(int id)
		{
			return context.Posts.FirstOrDefault(x => x.Id == id);
		}

		public IEnumerable<Post> GetByTitle(string title)
		{
			if (string.IsNullOrEmpty(title))
				throw new ArgumentException("Title is empty string.");

			return context.Posts.Where(x => x.Title.Contains(title)).OrderByDescending(x => x.Created);

		}

		public Post GetFirstByTitle(string title)
		{
			if (string.IsNullOrEmpty(title))
				throw new ArgumentException("Title is empty string.");

			return context.Posts.First(x => x.Title.Contains(title));
		}

		public IEnumerable<Post> GetByTag(string tag)
		{
			if (string.IsNullOrEmpty(tag))
				throw new ArgumentException("Tag is empty string.");

			return context.Posts.Where(x => x.Tags.Contains(tag)).OrderByDescending(x => x.Created);
		}

		public Post GetFirstByTag(string tag)
		{
			if (string.IsNullOrEmpty(tag))
				throw new ArgumentException("Tag is empty string.");

			return context.Posts.First(x => x.Tags.Contains(tag));
		}

		public IEnumerable<Post> SortByDate()
		{
			return context.Posts.OrderByDescending(x => x.Created);
		}

		public void Create(Post entity)
		{
			context.Posts.Add(entity);
		}

		public async Task Create(Post post, IFormFile image)
		{
			if (image != null)
				post.ImagePath = await fileManager.SaveImage(image);
			context.Posts.Add(post);
		}

		public async void Update(int id, Post entity)
		{
			await Update(id, entity, null);
		}

		public async Task Update(int id, Post post, IFormFile image)
		{
			if (post == null)
				throw new ArgumentNullException();

			Post oldPost = context.Posts.SingleOrDefault(x => x.Id == id) ??
					throw new ArgumentOutOfRangeException("Can't find and update item with id: " + id);

			if (image != null)
				post.ImagePath = await fileManager.SaveOrCreateImage(oldPost.ImagePath, post.ImagePath, image);

			AssignNewValues(post, oldPost);
		}

		private static void AssignNewValues(Post entity, Post oldPost)
		{
			oldPost.Title = entity.Title;
			oldPost.Content = entity.Content;
			oldPost.Excerpt = entity.Excerpt;
			oldPost.Public = entity.Public;

			if (!string.IsNullOrWhiteSpace(entity.ImagePath))
				oldPost.ImagePath = entity.ImagePath;

			oldPost.Tags = entity.Tags;
			oldPost.Updated = DateTime.Now;
		}


		public bool Delete(int id)
		{
			Post post = context.Posts.FirstOrDefault(x => x.Id == id);
			if (post != null)
			{
				if (!string.IsNullOrEmpty(post.ImagePath))
					fileManager.DeleteImage(post.ImagePath);

				context.Posts.Remove(post);
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
