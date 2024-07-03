namespace PhotoService.Services
{
	public interface IFileManager
	{
		Task<string> DuplicateAsync(string path, string suffix, CancellationToken cancellationToken = default);
	}
}
