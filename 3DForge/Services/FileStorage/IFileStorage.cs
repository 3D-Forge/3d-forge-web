using Backend3DForge.Models;

namespace Backend3DForge.Services.FileStorage
{
    public interface IFileStorage
    {
        // TO DO - this functions depends on UserModel and other models from db
        // Upload/Download file to print
        // Upload/Download preview file

        public Task<Stream> DownloadAvatarAsync(User user);
        public Task UploadAvatarAsync(User user, Stream fileStream, long fileSize = -1);

        public Task<Stream> DownloadFileAsync(string filename);
        public Task UploadFileAsync(string filename, Stream fileStream, long fileSize = -1);
        public Task DeleteFileAsync(string filename);
    }
}
