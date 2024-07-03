using Contracts.PhotoEvents;
using Contracts.UserEvents;
using MassTransit;
using PhotoService.Services;

namespace PhotoService.Consumers
{
	public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
	{
		private readonly IFileManager fileManager;
		private readonly IBus bus;

		public UserCreatedConsumer(IFileManager fileManager, IBus bus)
		{
			this.fileManager = fileManager;
			this.bus = bus;
		}

		public async Task Consume(ConsumeContext<UserCreatedEvent> context)
		{
			var message = context.Message;

			await Task.Delay(1000);
			var smallPhotoSource = await fileManager.DuplicateAsync(message.OriginalPhotoSource, "small");

			await Task.Delay(1000);
			var normalPhotoSource = await fileManager.DuplicateAsync(message.OriginalPhotoSource, "normal");

			await Task.Delay(1000);
			var bigPhotoSource = await fileManager.DuplicateAsync(message.OriginalPhotoSource, "big");

			await bus.Publish(new PhotoCreatedEvent
			{
				UserId = message.UserId,
				SmallPhotoSource = smallPhotoSource,
				NomarlPhotoSource = normalPhotoSource,
				BigPhotoSource = bigPhotoSource,
			});
		}
	}
}
