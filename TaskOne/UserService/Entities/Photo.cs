namespace UserService.Entities
{
	public class Photo
	{
		public Guid Id { get; set; }
		public string Source { get; set; }
		public PhotoType Type { get; set; }
	}
}
