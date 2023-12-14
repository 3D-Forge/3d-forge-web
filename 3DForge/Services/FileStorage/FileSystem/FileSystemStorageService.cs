using Backend3DForge.Models;
using Microsoft.Extensions.Options;

namespace Backend3DForge.Services.FileStorage.FileSystem
{
	public class FileSystemStorageService : IFileStorage
	{
		protected readonly FileSystemStorageConfigurationMetadata configuration;
		private string RootPath => Path.IsPathRooted(configuration.RootPath) ? configuration.RootPath : Path.Combine(Directory.GetCurrentDirectory(), configuration.RootPath);

		private string GetFullPath(string filename)
		{
			return Path.Combine(RootPath, filename);
		}

		public FileSystemStorageService(IOptions<FileSystemStorageConfigurationMetadata> configuration)
		{
			this.configuration = configuration.Value;

			if (!Directory.Exists(RootPath))
			{
				Directory.CreateDirectory(RootPath);
			}
		}

		public Task DeleteFileAsync(string filename)
		{
			if (filename == null)
			{
				throw new ArgumentNullException("filename");
			}

			if (!File.Exists(GetFullPath(filename)))
			{
				return Task.CompletedTask;
			}

			string dir = Path.GetDirectoryName(GetFullPath(filename));

			File.Delete(GetFullPath(filename));

			if (Directory.Exists(dir) && !Directory.EnumerateFileSystemEntries(dir).Any())
			{
				Directory.Delete(dir);
			}

			return Task.CompletedTask;
		}

		public Task<Stream> DownloadFileAsync(string filename)
		{
			if (filename == null)
			{
				throw new ArgumentNullException("filename");
			}

			if (!File.Exists(GetFullPath(filename)))
			{
				throw new FileNotFoundException("File not found", filename);
			}

			return Task.FromResult(new FileStream(GetFullPath(filename), FileMode.Open, FileAccess.Read) as Stream);
		}

		public async Task UploadFileAsync(string filename, Stream fileStream, long fileSize = -1)
		{
			if (filename == null)
			{
				throw new ArgumentNullException("filename");
			}

			if (fileStream == null)
			{
				throw new ArgumentNullException("fileStream");
			}

			// create path if it not exists
			string[] pathParts = filename.Split(Path.DirectorySeparatorChar);
			string fileFullPath = RootPath;
			for (int i = 0; i < pathParts.Length - 1; i++)
			{
				fileFullPath = Path.Combine(fileFullPath, pathParts[i]);
				if (!Directory.Exists(fileFullPath))
				{
					Directory.CreateDirectory(fileFullPath);
				}
			}
			fileFullPath = Path.Combine(fileFullPath, pathParts.Last());

			using (var fs = new FileStream(fileFullPath, FileMode.CreateNew, FileAccess.Write))
			{
				await fileStream.CopyToAsync(fs);
			}
		}

		public Task<Stream> DownloadAvatarAsync(User user)
		{
			try
			{
				return DownloadFileAsync($"{configuration.AvatarStoragePath}{Path.DirectorySeparatorChar}u{user.Id}.png");
			}
			catch (FileNotFoundException)
			{
				return Task.FromResult(new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "src", "img", "no-avatar.png"), FileMode.Open, FileAccess.Read) as Stream);
			}
		}

		public Task UploadAvatarAsync(User user, Stream fileStream, long fileSize = -1)
		{
			return UploadFileAsync(
				filename: $"{configuration.AvatarStoragePath}{Path.DirectorySeparatorChar}u{user.Id}.png",
				fileStream: fileStream,
				fileSize: fileSize);
		}

		public Task DeleteAvatarAsync(User user)
		{
			return DeleteFileAsync($"{configuration.AvatarStoragePath}{Path.DirectorySeparatorChar}u{user.Id}.png");
		}

		public Task<Stream> DownloadPreviewModel(CatalogModel catalogModel)
		{
			return DownloadFileAsync($"{configuration.PathToPreviewFiles}{Path.DirectorySeparatorChar}{catalogModel.Id}.{catalogModel.ModelExtensionId}");
		}

		public Task UploadPreviewModel(CatalogModel catalogModel, Stream fileStream, long fileSize = -1)
		{
			return UploadFileAsync(
					filename: $"{configuration.PathToPreviewFiles}{Path.DirectorySeparatorChar}{catalogModel.Id}.{catalogModel.ModelExtensionId}",
					fileStream: fileStream,
					fileSize: fileSize);
		}

		public Task DeletePreviewModel(CatalogModel catalogModel)
		{
			return DeleteFileAsync($"{configuration.PathToPreviewFiles}{Path.DirectorySeparatorChar}{catalogModel.Id}.{catalogModel.ModelExtensionId}");
		}

		public Task<Stream> DownloadPrintFile(CatalogModel catalogModel)
		{
			return DownloadFileAsync($"{configuration.PathToFilesToPrint}{Path.DirectorySeparatorChar}{catalogModel.Id}.{catalogModel.PrintExtensionId}");
		}

		public Task UploadPrintFile(CatalogModel catalogModel, Stream fileStream, long fileSize = -1)
		{
			return UploadFileAsync(
					filename: $"{configuration.PathToFilesToPrint}{Path.DirectorySeparatorChar}{catalogModel.Id}.{catalogModel.PrintExtensionId}",
					fileStream: fileStream,
					fileSize: fileSize);
		}

		public Task DeletePrintFile(CatalogModel catalogModel)
		{
			return DeleteFileAsync($"{configuration.PathToFilesToPrint}{Path.DirectorySeparatorChar}{catalogModel.Id}.{catalogModel.PrintExtensionId}");
		}

        public Task<Stream> Download3DModelsPicture(ModelPicture modelPicture)
        {
            return DownloadFileAsync($"{configuration.PathTo3DModelsPictures}{Path.DirectorySeparatorChar}{modelPicture.Id}.png");
        }

        public Task Upload3DModelsPicture(ModelPicture modelPicture, Stream fileStream, long fileSize = -1)
        {
            return UploadFileAsync(
				filename: $"{configuration.PathTo3DModelsPictures}{Path.DirectorySeparatorChar}{modelPicture.Id}.png",
				fileStream: fileStream,
				fileSize: fileSize);
        }

        public Task Delete3DModelsPicture(ModelPicture modelPicture)
        {
            return DeleteFileAsync($"{configuration.PathTo3DModelsPictures}{Path.DirectorySeparatorChar}{modelPicture.Id}.png");
        }

		public Task<Stream> DownloadPrintFile(OrderedModel orderedModel)
		{
			return DownloadFileAsync($"{configuration.PathToOrderedModels}{Path.DirectorySeparatorChar}{orderedModel.Id}.{orderedModel.PrintExtensionId}");
		}

		public Task UploadPrintFile(OrderedModel orderedModel, Stream fileStream, long fileSize = -1)
		{
			return UploadFileAsync(
					filename: $"{configuration.PathToOrderedModels}{Path.DirectorySeparatorChar}{orderedModel.Id}.{orderedModel.PrintExtensionId}",
					fileStream: fileStream,
					fileSize: fileSize);
		}

		public Task DeletePrintFile(OrderedModel catalogModel)
		{
			return DeleteFileAsync($"{configuration.PathToOrderedModels}{Path.DirectorySeparatorChar}{catalogModel.Id}.{catalogModel.PrintExtensionId}");
		}

        public Task<Stream> DownloadDefaultAvatar()
        {
            return Task.FromResult(new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "src", "img", "no-avatar.png"), FileMode.Open, FileAccess.Read) as Stream);
        }
    }
}
