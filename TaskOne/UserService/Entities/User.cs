namespace UserService.Entities
{
	public class User
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public List<Photo> Photos { get; set; }
		public bool IsConfirmed { get; set; }
	}
}
