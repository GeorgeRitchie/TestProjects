
namespace UserService.Services
{
	public class FileManager : IFileManager
	{
		private readonly string _photosBaseDirectory;

		public FileManager(IWebHostEnvironment env)
		{
			_photosBaseDirectory = Path.Combine(env.WebRootPath, "UserPhotos");
			Directory.CreateDirectory(_photosBaseDirectory);
		}

		public async Task<string> SaveFileAsync(Guid userId, IFormFile file)
		{
			if (file == null || file.Length == 0)
			{
				throw new ArgumentException("File is empty or null.", nameof(file));
			}

			string fileExtension = Path.GetExtension(file.FileName);
			if (fileExtension != ".jpg" && fileExtension != ".png")
			{
				throw new ArgumentException("Invalid file type.", nameof(file));
			}

			var currentUserFilesDir = Path.Combine(_photosBaseDirectory, userId.ToString());
			Directory.CreateDirectory(currentUserFilesDir);

			string fileName = $"{Guid.NewGuid()}{fileExtension}";
			string fullPath = Path.Combine(currentUserFilesDir, fileName);

			using (var stream = new FileStream(fullPath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			return fullPath;
		}
	}
}
