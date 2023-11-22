namespace Backend3DForge.Services.BackgroundWorker
{
    public class BackgroundWorkerConfigurationMetadata
    {
        public int MaxConcurrentTasks { get; set; } = 1;
        public int MaxTaskQueueSize { get; set; } = 1000;
    }
}
