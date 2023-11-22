namespace Backend3DForge.Services.BackgroundWorker
{
    public interface IBackgroundWorker
    {
        int QueueSize { get; }
        int RunningTasksCount { get; }
        int FinishedTasksCount { get; }

        public IBackgroundTask CreateTask(Func<object, object> action, object parameter);

        Task SubscribeToTaskInformation(string id, Stream stream);
    }
}
