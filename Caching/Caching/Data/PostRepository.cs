using Caching.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Caching.Data
{
	public class PostRepository(AppDbContext dbContext, IDistributedCache cache)
	{
		private DistributedCacheEntryOptions cacheOptions = new()
		{
			SlidingExpiration = TimeSpan.FromMinutes(3),
		};

		public Post Create(Post post)
		{
			dbContext.Posts.Add(post);
			return post;
		}

		public void Update(Post post)
		{
			dbContext.Posts.Update(post);
			cache.Remove(post.Id.ToString());
		}

		public void Delete(Post post)
		{
			dbContext.Posts.Remove(post);
			cache.Remove(post.Id.ToString());
		}

		public void Delete(Guid id)
		{
			var post = dbContext.Posts.SingleOrDefault(i => i.Id == id);

			if (post != null)
			{
				dbContext.Posts.Remove(post);
				cache.Remove(post.Id.ToString());
			}
		}

		public Post? GetById(Guid id)
		{
			var postStr = cache.GetString(id.ToString());
			Post? post = null;

			if (postStr == null)
			{
				post = dbContext.Posts.FirstOrDefault(x => x.Id == id);

				if (post != null)
				{
					cache.SetString(post.Id.ToString(), JsonConvert.SerializeObject(post), cacheOptions);
				}
			}
			else
			{
				post = JsonConvert.DeserializeObject<Post>(postStr);
			}

			return post;
		}

		public IQueryable<Post> GetAll()
		{
			return dbContext.Posts;
		}
	}
}
