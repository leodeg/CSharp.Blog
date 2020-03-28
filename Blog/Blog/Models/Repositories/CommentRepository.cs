using Blog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models.Repositories
{

	public class CommentRepository : ICommentRepository
	{
		private ApplicationDbContext context;

		public CommentRepository(ApplicationDbContext context)
		{
			this.context = context;
		}

		public IEnumerable<Comment> Get()
		{
			return context.Comments.OrderByDescending(x => x.Created);
		}

		public IEnumerable<Comment> GetByPost(int id)
		{
			return context.Comments.Where(c => c.PostId == id).OrderByDescending(x => x.Created);
		}

		public Comment Get(int id)
		{
			return context.Comments.FirstOrDefault(x => x.Id == id);
		}

		public void Create(Comment entity)
		{
			context.Comments.Add(entity);
		}

		public void Update(int id, Comment entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			Comment oldTag = context.Comments.FirstOrDefault(x => x.Id == id);
			if (entity == null)
				throw new ArgumentOutOfRangeException("Can't find and update item with id: " + id);

			oldTag.UserName = entity.UserName;
			oldTag.Message = entity.Message;
			oldTag.Updated = DateTime.Now;
		}

		public bool Delete(int id)
		{
			Comment comment = context.Comments.FirstOrDefault(x => x.Id == id);
			if (comment != null)
			{
				context.Comments.Remove(comment);
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
