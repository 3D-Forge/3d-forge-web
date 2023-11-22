namespace Backend3DForge.Services.TempStorage
{
    public class TempStorageConfigurationMetadata
    {
        public string TempStoragePath { get; set; } = "tmp";
        public TimeSpan ExpirationTime { get; set; } = TimeSpan.FromHours(1);

        public string FullPath => Path.Combine(Directory.GetCurrentDirectory(), TempStoragePath);
    }
}
