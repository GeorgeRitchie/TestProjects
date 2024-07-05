using Caching.Data;
using Caching.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Caching.Controllers
{
	[ApiController]
	[Route("[controller]/[action]")]
	public class PostController : ControllerBase
	{
		private readonly AppDbContext db;
		private readonly PostRepository postsRepository;

		public PostController(AppDbContext db, PostRepository postsRepository)
		{
			this.db = db;
			this.postsRepository = postsRepository;
		}

		[HttpPost]
		public Guid Create([FromBody] CreatePostDto input)
		{
			var post = new Post()
			{
				Id = Guid.NewGuid(),
				Title = input.Title,
				Content = input.Content,
			};

			postsRepository.Create(post);
			db.SaveChanges();

			return post.Id;
		}

		[HttpGet]
		public Post GetById([FromQuery] Guid id)
			=> postsRepository.GetById(id);

		[HttpGet]
		public IEnumerable<Post> GetAll()
			=> postsRepository.GetAll();

		[HttpPut]
		public ActionResult Update([FromBody] UpdatePostDto input)
		{
			var post = postsRepository.GetById(input.Id);

			if (post == null)
				return NotFound(input.Id);

			post.Title = input.Title;
			post.Content = input.Content;

			postsRepository.Update(post);
			db.SaveChanges();

			return Ok(post);
		}

		[HttpDelete]
		public ActionResult Delete([FromQuery] Guid id)
		{
			postsRepository.Delete(id);
			db.SaveChanges();
			return Ok();
		}
	}

	public class CreatePostDto
	{
		public string Title { get; set; }
		public string Content { get; set; }
	}

	public class UpdatePostDto
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
	}
}
