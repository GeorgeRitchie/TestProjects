namespace PhotoService.Services
{
	public class FileManager : IFileManager
	{
		public Task<string> DuplicateAsync(string path, string suffix, CancellationToken cancellationToken = default)
		{
			string directory = Path.GetDirectoryName(path);
			string fileName = Path.GetFileNameWithoutExtension(path);
			string extension = Path.GetExtension(path);

			string newFileName = $"{fileName}-{suffix}{extension}";
			string newFilePath = Path.Combine(directory, newFileName);

			// Copy the original file to the new file with the suffix
			File.Copy(path, newFilePath, overwrite: true);

			return Task.FromResult(newFilePath);
		}
	}
}
