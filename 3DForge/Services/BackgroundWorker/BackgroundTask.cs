namespace Backend3DForge.Services.BackgroundWorker
{
    public class BackgroundTask : IBackgroundTask
    {
        public string Id { get; set; }
        public Func<object, object> Action { get; set; }
        public object Parameter { get; set; }
        public TaskStatus Status { get; set; }
        public object? Result { get; set; } = null;
        public Task? Task { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public BackgroundTask(string id, Func<object, object> action, object parameter)
        {
            Id = id;
            Action = action;
            Status = TaskStatus.Created;
            Parameter = parameter;
        }
    }
}
