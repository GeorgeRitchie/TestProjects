namespace UserService.Services
{
	public interface IFileManager
	{
		Task<string> SaveFileAsync(Guid userId, IFormFile file);
	}
}
