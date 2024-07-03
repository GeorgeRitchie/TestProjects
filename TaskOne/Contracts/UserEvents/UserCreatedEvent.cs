namespace Contracts.UserEvents
{
	public class UserCreatedEvent
	{
		public Guid UserId { get; set; }
		public string OriginalPhotoSource { get; set; }
	}
}
