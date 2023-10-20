namespace Backend3DForge.Services.FileStorage.FTP
{
    public class FTPStorageConfigurationMetadata : DefaultConfiguration
    {
        public string Username { get; set; } = "anonimys";
        public string Password { get; set; } = "anonimys";
        public string Host { get; set; }
        public int Port { get; set; } = 21;
        public bool SFTP { get; set; } = false;

        public string UriPath => SFTP ? $"sftp://{Host}:{Port}" : $"ftp://{Host}:{Port}";

        public override string AvatarStoragePath { get; set; } = "Avatars";
        public override string PathToFilesToPrint { get; set; } = "FilesToPrint";
        public override string PathToPreviewFiles { get; set; } = "PreviewFiles";
    }
}
