using Backend3DForge.Models;
using Microsoft.Extensions.Options;
using System.Net;

namespace Backend3DForge.Services.FileStorage.FTP
{
    public class FTPStorageService : IFileStorage
    {
        protected readonly FTPStorageConfigurationMetadata configuration;
        protected readonly ILogger logger;

        public FTPStorageService(IOptions<FTPStorageConfigurationMetadata> configuration, ILogger logger)
        {
            this.configuration = configuration.Value;

            this.MkDirAsync(this.configuration.AvatarStoragePath).Wait();
            this.MkDirAsync(this.configuration.PathToPreviewFiles).Wait();
            this.MkDirAsync(this.configuration.PathToFilesToPrint).Wait();
            this.logger = logger;
        }

        public async Task DeleteFileAsync(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException(nameof(filename));
            }

            try
            {
                filename = NormalizePath(filename);
                string[] pathParts = filename.Split('/');

                FtpWebRequest? request = CreateFTPConnection(filename, WebRequestMethods.Ftp.DeleteFile);
                WebResponse response = await request.GetResponseAsync();

                response.Close();

                // delete empty directories
                for (int i = pathParts.Length - 1; i > 0; i--)
                {
                    string path = string.Join("/", pathParts.Take(i));
                    if (await IsDirectoryEmptyAsync(path))
                    {
                        await DeleteDirectoryAsync(path);
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.Log(LogLevel.Error, ex.ToString());
            }
        }

        public async Task<Stream> DownloadFileAsync(string filename)
        {
            FtpWebRequest? request = CreateFTPConnection(filename, WebRequestMethods.Ftp.DownloadFile);

            FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();
            return response.GetResponseStream();
        }

        public async Task UploadFileAsync(string filename, Stream fileStream, long fileSize = -1)
        {
            if (fileStream == null)
            {
                throw new ArgumentNullException(nameof(fileStream));
            }

            filename = NormalizePath(filename);

            string[] pathParts = filename.Split('/');
            string path = string.Empty;
            for (int i = 0; i < pathParts.Length - 1; i++)
            {
                path = $"{path}/{pathParts[i]}";
                if (!await IsDirectoryExists(path))
                {
                    await MkDirAsync(path);
                }
            }

            FtpWebRequest request = CreateFTPConnection(filename, WebRequestMethods.Ftp.UploadFile);

            request.ContentLength = fileSize > 0 ? fileSize : fileStream.Length;

            Stream ftpStream = request.GetRequestStream();
            await fileStream.CopyToAsync(ftpStream);
            ftpStream.Close();
        }

        protected async Task<bool> IsDirectoryExists(string directoryPath)
        {
            try
            {
                FtpWebRequest request = CreateFTPConnection(directoryPath, WebRequestMethods.Ftp.ListDirectoryDetails);
                using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
                {
                    return true;
                }
            }
            catch (WebException)
            {
                return false;
            }
        }

        protected async Task MkDirAsync(string directoryPath)
        {
            FtpWebRequest request = CreateFTPConnection(directoryPath, WebRequestMethods.Ftp.MakeDirectory);

            var response = await request.GetResponseAsync();
            response.Close();
        }

        protected async Task<bool> IsDirectoryEmptyAsync(string directoryPath)
        {
            try
            {
                FtpWebRequest request = CreateFTPConnection(directoryPath, WebRequestMethods.Ftp.ListDirectory);

                using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
                using (Stream responseStream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    string? line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                            return false;
                    }
                }

                return true;
            }
            catch (WebException)
            {
                return false;
            }
        }

        protected async Task DeleteDirectoryAsync(string directoryPath)
        {
            FtpWebRequest request = CreateFTPConnection(directoryPath, WebRequestMethods.Ftp.RemoveDirectory);

            FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();
            response.Close();
        }

        protected FtpWebRequest CreateFTPConnection(string path, string method)
        {
            path = NormalizePath(path);
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"{configuration.UriPath}/{path}");
            request.Method = method;
            request.Credentials = new NetworkCredential(configuration.Username, configuration.Password);

            return request;
        }

        protected string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (path.StartsWith("/"))
            {
                path = path.Substring(1)
                    .TrimEnd('/');
            }
            return path.Replace("\\", "/");
        }

        public async Task<Stream> DownloadAvatarAsync(User user)
        {
            Stream stream = null;
            try
            {
                stream = await DownloadFileAsync($"{configuration.AvatarStoragePath}{Path.DirectorySeparatorChar}u{user.Id}.png");
            }
            catch (WebException ex)
            {
                stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "src", "img", "no-avatar.png"), FileMode.Open, FileAccess.Read);
            }
            return stream;
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
    }
}
