using Backend3DForge.Models;

namespace Backend3DForge.Services.FileStorage
{
    public interface IFileStorage
    {
        public Task<Stream> DownloadPreviewModel(CatalogModel catalogModel);
        public Task UploadPreviewModel(CatalogModel catalogModel, Stream fileStream, long fileSize = -1);
        public Task DeletePreviewModel(CatalogModel catalogModel);

        public Task<Stream> DownloadPrintFile(CatalogModel catalogModel);
        public Task UploadPrintFile(CatalogModel catalogModel, Stream fileStream, long fileSize = -1);
        public Task DeletePrintFile(CatalogModel catalogModel);

        public Task<Stream> DownloadAvatarAsync(User user);
        public Task UploadAvatarAsync(User user, Stream fileStream, long fileSize = -1);
        public Task DeleteAvatarAsync(User user);

        public Task<Stream> DownloadFileAsync(string filename);
        public Task UploadFileAsync(string filename, Stream fileStream, long fileSize = -1);
        public Task DeleteFileAsync(string filename);
    }
}
