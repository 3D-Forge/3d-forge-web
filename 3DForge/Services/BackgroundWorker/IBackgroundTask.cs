namespace Backend3DForge.Services.BackgroundWorker
{
    public interface IBackgroundTask
    {
        public string Id { get; set; }
        public TaskStatus Status { get; set; }
        public object? Result { get; set; }
    }
}
