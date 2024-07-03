using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Entities;
using UserService.Models.Request;
using UserService.Models.Response;
using UserService.Services;
using MassTransit;
using Contracts.UserEvents;

namespace UserService.Controllers
{
	[ApiController]
	[Route("[controller]/[action]")]
	public class UserController : ControllerBase
	{
		private readonly AppDbContext _appDbContext;
		private readonly IFileManager fileManager;
		private readonly IBus bus;

		public UserController(AppDbContext appDbContext, IFileManager fileManager, IBus bus)
		{
			_appDbContext = appDbContext;
			this.fileManager = fileManager;
			this.bus = bus;
		}

		[HttpGet]
		public async Task<ActionResult<UserDto>> GetUserById([FromQuery] Guid id)
		{
			var user = await _appDbContext.Users.Include(i => i.Photos).FirstOrDefaultAsync(i => i.Id == id);

			if (user == null)
				return NotFound($"User with Id {id} not found");

			if (user.IsConfirmed == false)
				return BadRequest($"Your profile is not processed yet.");

			return Ok(new UserDto
			{
				Id = user.Id,
				Name = user.Name,
				Email = user.Email,
				PhotoSources = user.Photos.Select(i => i.Source).ToList(),
			});
		}

		[HttpPost]
		public async Task<ActionResult<Guid>> CreateUser([FromForm] CreateUser userDto)
		{
			var user = new User()
			{
				Id = Guid.NewGuid(),
				Name = userDto.Name,
				Email = userDto.Email,
				IsConfirmed = false,
			};

			var originalPhotoPath = await fileManager.SaveFileAsync(user.Id, userDto.Photo);

			user.Photos.Add(new()
			{
				Id = Guid.NewGuid(),
				Source = originalPhotoPath,
				Type = PhotoType.Original,
			});

			_appDbContext.Users.Add(user);
			_appDbContext.SaveChanges();

			await bus.Publish(new UserCreatedEvent
			{
				UserId = user.Id,
				OriginalPhotoSource = user.Photos[0].Source,
			});

			return user.Id;
		}
	}
}
