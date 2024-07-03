namespace Contracts.PhotoEvents
{
	public class PhotoCreatedEvent
	{
		public Guid UserId { get; set; }
		public string SmallPhotoSource { get; set; }
		public string NomarlPhotoSource { get; set; }
		public string BigPhotoSource { get; set; }
	}
}
