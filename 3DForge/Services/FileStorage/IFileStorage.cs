namespace Backend3DForge.Services.FileStorage
{
    public interface IFileStorage
    {
        // TO DO - this functions depends on UserModel and other models from db
        // Upload/Download avatar
        // Upload/Download file to print
        // Upload/Download preview file

        public Task<Stream> DownloadFileAsync(string filename);
        public Task UploadFileAsync(string filename, Stream fileStream, int fileSize = -1);
        public Task DeleteFileAsync(string filename);
    }
}
