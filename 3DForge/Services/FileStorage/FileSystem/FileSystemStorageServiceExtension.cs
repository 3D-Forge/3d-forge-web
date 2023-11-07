namespace Backend3DForge.Services.FileStorage.FileSystem
{
	public static class FileSystemStorageServiceExtension
	{
		public static IServiceCollection AddFSFileStorage(this IServiceCollection services, Action<FileSystemStorageConfigurationMetadata>? settings = null)
		{
			services.AddSingleton<IFileStorage, FileSystemStorageService>();
			if (settings != null)
				services.Configure(settings);
			return services;
		}
	}
}
