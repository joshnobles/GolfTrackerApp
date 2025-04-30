using GolfProgressTracker.Web.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace GolfProgressTracker.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            RegisterServices(builder);

            var app = builder.Build();

            ConfigureApplication(app);

            app.Run();
        }

        private static void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddRazorPages();

            builder.Services.AddDbContext<Context>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("SQLite"));
            });
        }

        private static void ConfigureApplication(WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();
        }
    }
}
