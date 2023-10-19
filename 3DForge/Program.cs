using Backend3DForge.Services.Email;

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