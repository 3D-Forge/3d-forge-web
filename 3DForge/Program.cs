using Backend3DForge.Services.Email;

#if STORAGE_TYPE_FILESYSTEM
using Backend3DForge.Services.FileStorage.FileSystem;
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

            app.Run();
        }
    }
}