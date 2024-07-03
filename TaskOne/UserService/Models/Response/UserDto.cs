namespace UserService.Models.Response
{
	public class UserDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public List<string> PhotoSources { get; set; }
	}
}
