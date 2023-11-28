using Backend3DForge.Services.BackgroundWorker;
using Microsoft.AspNetCore.Mvc;

namespace Backend3DForge.Controllers
{
    [Route("api/task")]
    [ApiController]
    public class TaskController : BaseController
    {
        protected readonly IBackgroundWorker backgroundWorker;
        protected readonly ILogger logger;

        public TaskController(DbApp db, IBackgroundWorker backgroundWorker, ILogger logger) : base(db)
        {
            this.backgroundWorker = backgroundWorker;
            this.logger = logger;
        }

        [HttpGet("{id}")]
        public void Get(string id)
        {
            try
            {
                backgroundWorker.SubscribeToTaskInformation(id, Response);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                logger.LogDebug(ex, $"id: '{id}'");
            }
        }

        [HttpGet("info")]
        public IActionResult GetQueue()
        {
            return Ok(new
            {
                QueueSize = backgroundWorker.QueueSize,
                RunningTasksCount = backgroundWorker.RunningTasksCount,
                FinishedTasksCount = backgroundWorker.FinishedTasksCount
            });
        }
    }
}
