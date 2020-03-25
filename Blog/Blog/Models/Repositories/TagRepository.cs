using Blog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models.Repositories
{
	public class TagRepository : ITagRepository
	{
		private ApplicationDbContext context;

		public TagRepository(ApplicationDbContext context)
		{
			this.context = context;
		}

		public IEnumerable<Tag> Get()
		{
			return context.Tags.OrderBy(x => x.Name);
		}

		public Tag Get(int id)
		{
			return context.Tags.FirstOrDefault(x => x.Id == id);
		}

		public Tag Get(string tag)
		{
			return context.Tags.FirstOrDefault(x => x.Name == tag);
		}

		public void Create(Tag entity)
		{
			context.Tags.Add(entity);
		}

		public void Create(string tag)
		{
			if (Get(tag) == default)
				context.Tags.Add(new Tag() { Name = tag.Trim() });
		}

		public void Update(int id, Tag entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			Tag oldTag = context.Tags.FirstOrDefault(x => x.Id == id);
			if (entity == null)
				throw new ArgumentOutOfRangeException("Can't find and update item with id: " + id);

			oldTag.Name = entity.Name;
			oldTag.Updated = DateTime.Now;
		}

		public bool Delete(int id)
		{
			Tag tag = context.Tags.FirstOrDefault(x => x.Id == id);
			if (tag != null)
			{
				context.Tags.Remove(tag);
				return true;
			}
			return false;
		}

		public bool Delete(string tag)
		{
			Tag tagToDelete = Get(tag);
			if (tagToDelete != null)
			{
				context.Tags.Remove(tagToDelete);
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
