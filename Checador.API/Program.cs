using Microsoft.EntityFrameworkCore;
using Checador.API;

namespace Checador.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration
                .SetBasePath(Directory.GetCurrentDirectory ())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 104857600;
            });

            builder.Services.ConfigurandoDependencias(builder.Configuration);

            var app = builder.Build();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowAll");

            if (app.Environment.IsDevelopment())
            {
                app.MapControllers();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
