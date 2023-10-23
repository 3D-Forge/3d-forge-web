using Backend3DForge.Services.Email;

#if STORAGE_TYPE_FILESYSTEM
using Backend3DForge.Services.FileStorage.FileSystem;
using Microsoft.EntityFrameworkCore;

#else
using Backend3DForge.Services.FileStorage.FTP;
#endif

namespace Backend3DForge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSqlServer<DbApp>(builder.Configuration["ConnectionStrings:WebApiDatabase"]);

			builder.Services.AddControllersWithViews();

            // Register Email Service
            builder.Services.AddEmailService(e =>
            {
                e.Email = builder.Configuration["Services:Email:EmailAddress"];
                e.SenderName = builder.Configuration["Services:Email:SenderName"];
                e.Password = builder.Configuration["Services:Email:Password"];
                e.Host = builder.Configuration["Services:Email:Host"];
                e.Port = int.Parse(builder.Configuration["Services:Email:Port"]);
                e.EmailTemplatesFoulder = builder.Configuration["Services:Email:EmailTemplatesFoulder"];
            });


#if STORAGE_TYPE_FILESYSTEM
            // Register File Storage Service
            builder.Services.AddFSFileStorage(o =>
            {
                o.AvatarStoragePath = builder.Configuration["Services:FileSystem:AvatarStoragePath"];
                o.PathToFilesToPrint = builder.Configuration["Services:FileSystem:PathToFilesToPrint"];
                o.PathToPreviewFiles = builder.Configuration["Services:FileSystem:PathToPreviewFiles"];
            });
#else

            builder.Services.AddFTPFileStorage(o =>
            {
                o.Host = builder.Configuration["Services:FTP:Host"];
                o.Port = int.Parse(builder.Configuration["Services:FTP:Port"]);
                o.Username = builder.Configuration["Services:FTP:Username"];
                o.Password = builder.Configuration["Services:FTP:Password"];
                o.SFTP = bool.Parse(builder.Configuration["Services:FTP:SFTP"]);

                o.AvatarStoragePath = builder.Configuration["Services:FileSystem:AvatarStoragePath"];
                o.PathToFilesToPrint = builder.Configuration["Services:FileSystem:PathToFilesToPrint"];
                o.PathToPreviewFiles = builder.Configuration["Services:FileSystem:PathToPreviewFiles"];
            });
#endif

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");

            app.MapFallbackToFile("index.html");

			/*using (var db = new BloggingContext())
			{
				// Create and save a new Blog
				Console.Write("Enter a name for a new Blog: ");
				var name = Console.ReadLine();

				var blog = new Blog { Name = name };
				db.Blogs.Add(blog);
				db.SaveChanges();

				// Display all Blogs from the database
				var query = from b in db.Blogs
							orderby b.Name
							select b;

				Console.WriteLine("All blogs in the database:");
				foreach (var item in query)
				{
					Console.WriteLine(item.Name);
				}

				Console.WriteLine("Press any key to exit...");
				Console.ReadKey();
			}*/

			

			app.Run();
		}
    }
	public class Blog
	{
		public int BlogId { get; set; }
		public string Name { get; set; }

		public virtual List<Post> Posts { get; set; }
	}

	public class Post
	{
		public int PostId { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }

		public int BlogId { get; set; }
		public virtual Blog Blog { get; set; }
	}

	public class BloggingContext : DbContext
	{
		public DbSet<Blog> Blogs { get; set; }
		public DbSet<Post> Posts { get; set; }
	}
}