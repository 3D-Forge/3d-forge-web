using Azure;
using Microsoft.Extensions.Options;
using NuGet.Packaging;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Backend3DForge.Services.BackgroundWorker
{
    public class BackgroundWorkerService : IBackgroundWorker
    {
        protected readonly BackgroundWorkerConfigurationMetadata configuration;

        private List<BackgroundTask> queue;
        private List<BackgroundTask> runningTasks;
        private List<BackgroundTask> finishedTasks;

        private ManualResetEventSlim pauseEvent = new ManualResetEventSlim(true);

        private Task workingFlow;
        private Task cleanFlow;

        public BackgroundWorkerService(IOptions<BackgroundWorkerConfigurationMetadata> configuration)
        {
            this.configuration = configuration.Value;

            this.queue = new List<BackgroundTask>((int)(this.configuration.MaxTaskQueueSize * 0.2));
            this.runningTasks = new List<BackgroundTask>(this.configuration.MaxConcurrentTasks);
            this.finishedTasks = new List<BackgroundTask>(this.configuration.MaxConcurrentTasks / 2);
            this.cleanFlow = CleanFlow();
            this.workingFlow = WorkingFlow();
        }

        public int QueueSize => queue.Count;

        public int RunningTasksCount => runningTasks.Count;

        public int FinishedTasksCount => finishedTasks.Count;

        public IBackgroundTask CreateTask(Func<object[], object> action, object[] args)
        {
            BackgroundTask backgroundTask = new BackgroundTask(Guid.NewGuid().ToString(), action, args);

            queue.Add(backgroundTask);
            pauseEvent.Set();
            return backgroundTask;
        }

        public async Task SubscribeToTaskInformation(string id, HttpResponse response)
        {
            var task = queue.SingleOrDefault(p => p.Id == id);
            if (task == null)
            {
                task = this.runningTasks.SingleOrDefault(p => p.Id == id);
                if (task == null)
                {
                    task = this.finishedTasks.SingleOrDefault(p => p.Id == id);
                    if (task == null)
                    {
                        response.StatusCode = 404;
                        throw new KeyNotFoundException(id);
                    }
                    response.StatusCode = 200;
                    response.Headers.Add("Content-Type", "text/event-stream");
                    response.Headers.Add("Cache-Control", "no-cache");
                    response.Headers.Add("Connection", "keep-alive");
                    await response.WriteAsync($"data: {{\"id\":\"{id}\",\"status\":\"{Enum.GetName(task.Status)}\",\"posInQueue\":-1,\"queueSize\":{queue.Count()},\"result\":{JsonSerializer.Serialize(task.Result)}}}\r\r");
                    return;
                }
            }
            response.StatusCode = 200;
            response.Headers.Add("Content-Type", "text/event-stream");
            response.Headers.Add("Cache-Control", "no-cache");
            response.Headers.Add("Connection", "keep-alive");

            int posInQueue = 0;
            try
            {
                while (task.Status < TaskStatus.Canceled)
                {
                    if (posInQueue >= 0)
                    {
                        // I think this line isn't optimized. Actions must be used
                        posInQueue = queue.IndexOf(task);
                    }
                    // {id}|{Enum.GetName(task.Status)}|{posInQueue} to JSON
                    await response.WriteAsync($"data: {{\"id\":\"{id}\",\"status\":\"{Enum.GetName(task.Status)}\",\"posInQueue\":{posInQueue},\"queueSize\":{queue.Count()}}}\r\r");
                    Task.Delay(TimeSpan.FromSeconds(1)).Wait();
                }

                await response.WriteAsync($"data: {{\"id\":\"{id}\",\"status\":\"{Enum.GetName(task.Status)}\",\"posInQueue\":{posInQueue},\"queueSize\":{queue.Count()},\"result\":{JsonSerializer.Serialize(task.Result)}}}\r\r");
                await response.Body.FlushAsync();
            }
            catch (Exception ex)
            {
                return;
            }

        }

        protected Task WorkingFlow()
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    pauseEvent.Wait();
                    // handel runningTasks
                    if (this.queue.Count() > 0 && this.runningTasks.Count < this.configuration.MaxConcurrentTasks)
                    {
                        // get first (this.configuration.MaxConcurrentTasks - this.runningTasks.Count) items from this.queue
                        var tasksForCopyToRunningTasks = this.queue.Take(this.configuration.MaxConcurrentTasks - this.runningTasks.Count);

                        for (int i = 0; i < tasksForCopyToRunningTasks.Count(); i++)
                        {
                            var element = tasksForCopyToRunningTasks.ElementAt(i);
                            this.runningTasks.Add(element);
                            this.queue.Remove(element);
                            element.Status = TaskStatus.WaitingToRun;
                            element.Task = Task.Run(() =>
                            {
                                element.Status = TaskStatus.Running;
                                element.StartTime = DateTime.UtcNow;
                                try
                                {
                                    element.Result = element.Action.Invoke(element.Parameter);
                                    element.Status = TaskStatus.Canceled;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.ToString());
                                    element.Status = TaskStatus.Faulted;
                                }
                                element.EndTime = DateTime.UtcNow;
                                this.finishedTasks.Add(element);
                                this.runningTasks.Remove(element);
                            });
                        }
                    }
                    if (this.queue.Count == 0)
                    {
                        pauseEvent.Reset();
                    }
                    else
                    {
                        Task.Delay(TimeSpan.FromSeconds(3)).Wait();
                    }
                }
            });
        }

        private Task CleanFlow()
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    if (this.finishedTasks.Count > 0)
                    {
                        var oldTasks = this.finishedTasks.Where(p => p.EndTime < DateTime.UtcNow - TimeSpan.FromMinutes(1));
                        for (int i = 0; i < oldTasks.Count(); i++)
                        {
                            var element = oldTasks.ElementAt(i);
                            this.finishedTasks.Remove(element);
                        }
                    }
                    Task.Delay(TimeSpan.FromSeconds(15)).Wait();
                }
            });
        }
    }
}
