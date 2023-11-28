namespace Backend3DForge.Services.BackgroundWorker
{
    public interface IBackgroundWorker
    {
        int QueueSize { get; }
        int RunningTasksCount { get; }
        int FinishedTasksCount { get; }

        public IBackgroundTask CreateTask(Func<object[], object> action, object[] parameters);

        Task SubscribeToTaskInformation(string id, HttpResponse response);
    }
}
