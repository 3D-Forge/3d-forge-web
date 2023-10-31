using Microsoft.AspNetCore.Mvc;

namespace Backend3DForge.Controllers
{
	[ApiController]
	[Route("/test/db")]
	public class DbTestController : Controller
	{
		protected readonly DbApp db;

		public DbTestController(DbApp db)
		{
			this.db = db;
		}

		[HttpGet]
		public IActionResult Get()
		{
            return Ok(db.Users);
		}
	}
}
