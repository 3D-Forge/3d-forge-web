namespace Backend3DForge.Services.TempStorage
{
    public static class TempStorageExtension
    {
        public static IServiceCollection AddTempStorage(this IServiceCollection services, Action<TempStorageConfigurationMetadata> action)
        {
            services.AddSingleton<ITempStorage, TempStorageService>();
            services.Configure(action);
            return services;
        }
    }
}
