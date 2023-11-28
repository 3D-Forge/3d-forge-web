using Microsoft.Extensions.Options;

namespace Backend3DForge.Services.TempStorage
{
    public class TempStorageService : ITempStorage, IDisposable
    {
        protected readonly TempStorageConfigurationMetadata configuration;

        protected Task cleanUpTask;

        public TempStorageService(IOptions<TempStorageConfigurationMetadata> configuration)
        {
            this.configuration = configuration.Value;

            if(!Directory.Exists(this.configuration.FullPath))
            {
                Directory.CreateDirectory(this.configuration.FullPath);
            }

            // clean up old files
            foreach(var file in Directory.GetFiles(this.configuration.FullPath))
            {
                File.Delete(file);
            }

            cleanUpTask = CleanUpFlow();
        }

        public void DeleteFileAsync(string fileName)
        {
            if(File.Exists(Path.Combine(this.configuration.FullPath, fileName)))
            {
                File.Delete(Path.Combine(this.configuration.FullPath, fileName));
            }
        }

        public Stream DownloadFileAsync(string fileName)
        {
            return new FileStream(Path.Combine(this.configuration.FullPath, fileName), FileMode.Open, FileAccess.Read);
        }

        public async Task<string> UploadFileAsync(Stream stream)
        {
            var key = Guid.NewGuid().ToString() + DateTime.UtcNow.Ticks.ToString() + ".tmp";
            await UploadFileAsync(key, stream);
            return key;
        }

        public async Task UploadFileAsync(string fileName, Stream stream)
        {
            if(File.Exists(Path.Combine(this.configuration.FullPath, fileName)))
            {
                throw new Exception("File already exists");
            }

            using(var fileStream = new FileStream(Path.Combine(this.configuration.FullPath, fileName), FileMode.CreateNew, FileAccess.Write))
            {
                await stream.CopyToAsync(fileStream);
            }
        }

        public void Dispose()
        {
            cleanUpTask.Dispose();
        }

        public Task CleanUpFlow()
        {
            return Task.Run(async () =>
            {
                while (true)
                {
                    foreach (var file in Directory.GetFiles(this.configuration.FullPath))
                    {
                        var fileInfo = new FileInfo(file);
                        if(fileInfo.Exists && fileInfo.CreationTimeUtc + this.configuration.ExpirationTime < DateTime.UtcNow)
                        {
                            fileInfo.Delete();
                        }
                    }

                    await Task.Delay(TimeSpan.FromSeconds(15));
                }
            });
        }
    }
}
