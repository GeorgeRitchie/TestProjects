using Contracts.PhotoEvents;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Entities;

namespace UserService.Consumers
{
	public class PhotoCreatedConsumer : IConsumer<PhotoCreatedEvent>
	{
		private readonly AppDbContext dbContext;

		public PhotoCreatedConsumer(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task Consume(ConsumeContext<PhotoCreatedEvent> context)
		{
			var user = await dbContext.Users.Include(i => i.Photos).SingleOrDefaultAsync(i => i.Id == context.Message.UserId);

			if (user == null)
				return;

			user.IsConfirmed = true;

			List<Photo> newPhotos = [
				new()
				{
					Id = Guid.NewGuid(),
					Source = context.Message.SmallPhotoSource,
					Type = PhotoType.Small,
				},
				new()
				{
					Id = Guid.NewGuid(),
					Source = context.Message.NomarlPhotoSource,
					Type = PhotoType.Medium,
				},
				new()
				{
					Id = Guid.NewGuid(),
					Source = context.Message.BigPhotoSource,
					Type = PhotoType.Large,
				}
			];

			user.Photos.AddRange(newPhotos);
			dbContext.Photos.AddRange(newPhotos);

			dbContext.SaveChanges();
		}
	}
}
