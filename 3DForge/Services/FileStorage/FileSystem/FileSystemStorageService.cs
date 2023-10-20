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

            if(!Directory.Exists(RootPath))
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

            return Task.FromResult(new FileStream(GetFullPath(filename), FileMode.Open, FileAccess.Read) as Stream);
        }

        public async Task UploadFileAsync(string filename, Stream fileStream, int fileSize = -1)
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
    }
}
