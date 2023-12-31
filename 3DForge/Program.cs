//#if STORAGE_TYPE_FILESYSTEM
#if true
using Backend3DForge.Services.FileStorage.FileSystem;
#else
using Backend3DForge.Services.FileStorage.FTP;
#endif

using Backend3DForge.Attributes;
using Backend3DForge.Services.Email;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Backend3DForge.Services.ModelCalculator;
using Backend3DForge.Services.BackgroundWorker;
using Backend3DForge.Services.TempStorage;

namespace Backend3DForge
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			if(!builder.Environment.IsDevelopment())
			{
                builder.WebHost.UseUrls("https://0.0.0.0:8686");
            }

			builder.Services.AddSqlServer<DbApp>(builder.Configuration["ConnectionStrings:WebApiDatabase"]);

			builder.Services.AddControllersWithViews();

			builder.Services.AddMemoryCache();

			builder.Services.AddTempStorage(options =>
			{
				options.ExpirationTime = TimeSpan.FromMinutes(30);
				options.TempStoragePath = "temp";
            });

			builder.Services.AddBackgroundWorker((options) =>
			{
				options.MaxTaskQueueSize = 1000;
				options.MaxConcurrentTasks = 5;
			});

            builder.Services.AddSwaggerGen(p =>
			{
				p.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
				{
					Title = "",
					Version = "v1"
				});
			});
			builder.Services.AddSwaggerGen();

			builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.LoginPath = "/login";
				});

			builder.Services.AddSingleton<IAuthorizationHandler, CanAdministrateForumHandler>();
			builder.Services.AddSingleton<IAuthorizationHandler, CanRetrieveDeliveryHandler>();
			builder.Services.AddSingleton<IAuthorizationHandler, CanModerateCatalogHandler>();
			builder.Services.AddSingleton<IAuthorizationHandler, CanAdministrateSystemHandler>();

			builder.Services.AddTransient<IModelCalculator, ModelCalculator>();

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


#if true
			// Register File Storage Service
			builder.Services.AddFSFileStorage(o =>
			{
				o.AvatarStoragePath = builder.Configuration["Services:FileSystem:AvatarStoragePath"];
				o.PathToFilesToPrint = builder.Configuration["Services:FileSystem:PathToFilesToPrint"];
				o.PathToPreviewFiles = builder.Configuration["Services:FileSystem:PathToPreviewFiles"];
				o.PathTo3DModelsPictures = builder.Configuration["Services:FileSystem:PathTo3DModelsPictures"];
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
				o.PathTo3DModelsPictures = builder.Configuration["Services:FileSystem:PathTo3DModelsPictures"];
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

			app.UseSwagger();
			app.UseSwaggerUI(p =>
			{
				p.SwaggerEndpoint("/swagger/v1/swagger.json", "");
			});

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller}/{action=Index}/{id?}");

			app.MapFallbackToFile("index.html");

			app.Run();
		}
	}
}