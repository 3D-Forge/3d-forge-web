using Backend3DForge.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend3DForge
{
    public class DbApp : DbContext
	{
		protected readonly IConfiguration Configuration;

		public DbApp(DbContextOptions<DbApp> options) : base(options)
		{
			Database.EnsureCreated();
			Tools.InitSystem.Init(this);
		}

		public DbSet<User> Users { get; set; }
	}
}
