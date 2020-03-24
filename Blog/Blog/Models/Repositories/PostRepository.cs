using Blog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models.Repositories
{

	public class PostRepository : IRepository<Post>
	{
		private ApplicationDbContext context;

		public PostRepository(ApplicationDbContext context)
		{
			this.context = context;
		}

		public IEnumerable<Post> Get()
		{
			return context.Posts;
		}

		public Post Get(int id)
		{
			return context.Posts.FirstOrDefault(x => x.Id == id);
		}

		public void Create(Post entity)
		{
			context.Posts.Add(entity);
		}

		public void Update(int id, Post entity)
		{
			if (entity == null) throw new ArgumentNullException();

			Post oldPost = context.Posts.SingleOrDefault(x => x.Id == id);
			if (entity == null) throw new ArgumentOutOfRangeException("Can't find and update item with id: " + id);

			oldPost.Title = entity.Title;
			oldPost.Content = entity.Content;
			oldPost.Excerpt = entity.Excerpt;
			oldPost.Public = entity.Public;

			if (!string.IsNullOrWhiteSpace(entity.CoverImagePath))
				oldPost.CoverImagePath = entity.CoverImagePath;

			oldPost.Updated = DateTime.Now;
		}

		public bool Delete(int id)
		{
			Post post = context.Posts.FirstOrDefault(x => x.Id == id);
			if (post != null)
			{
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
