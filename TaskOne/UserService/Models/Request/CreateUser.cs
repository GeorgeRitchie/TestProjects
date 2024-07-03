namespace UserService.Models.Request
{
	public class CreateUser
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public IFormFile Photo { get; set; }
	}
}
