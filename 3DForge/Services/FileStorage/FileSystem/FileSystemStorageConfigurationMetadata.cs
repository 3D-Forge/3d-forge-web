namespace Backend3DForge.Services.FileStorage.FileSystem
{
    public class FileSystemStorageConfigurationMetadata : DefaultConfiguration
    {
        public string RootPath { get; set; } = "Storage";
        public override string AvatarStoragePath { get; set; } = "Avatars";
        public override string PathToFilesToPrint { get; set; } = "FilesToPrint";
        public override string PathToPreviewFiles { get; set; } = "PreviewFiles";
    }
}
