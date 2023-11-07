namespace Backend3DForge.Services.FileStorage.FTP
{
	public static class FTPStorageServiceExtension
	{
		public static IServiceCollection AddFTPFileStorage(this IServiceCollection services, Action<FTPStorageConfigurationMetadata> settings)
		{
			services.AddSingleton<IFileStorage, FTPStorageService>();
			services.Configure(settings);
			return services;
		}
	}
}
