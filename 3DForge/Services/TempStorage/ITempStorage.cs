namespace Backend3DForge.Services.TempStorage
{
    public interface ITempStorage
    {
        Task<string> UploadFileAsync(Stream stream);
        Task UploadFileAsync(string fileName, Stream stream);
        Stream DownloadFileAsync(string fileName);
        void DeleteFileAsync(string fileName);
    }
}
