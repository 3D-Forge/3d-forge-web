namespace Backend3DForge.Services.BackgroundWorker
{
    public static class BackgroundWorkerExtension
    {
        public static IServiceCollection AddBackgroundWorker(this IServiceCollection services, Action<BackgroundWorkerConfigurationMetadata>? config)
        {
            services.AddSingleton<IBackgroundWorker, BackgroundWorkerService>();
            if (config is not null)
            {
                services.Configure(config);
            }
            return services;
        }
    }
}
