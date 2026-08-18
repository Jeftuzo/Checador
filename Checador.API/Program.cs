using Microsoft.EntityFrameworkCore;
using Checador.API;

namespace Checador.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppContext.SetSwitch(
                "Npgsql.EnableLegacyTimestampBehavior",
                true);

            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(
                    "appsettings.json",
                    optional: false,
                    reloadOnChange: true)
                .AddEnvironmentVariables();

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 104857600;
            });

            builder.Services.ConfigurandoDependencias(
                builder.Configuration);

            var app = builder.Build();

            app.UseRouting();

            app.UseCors("AllowAll");

            // Desactivado durante desarrollo.
            // Evita redirecciones de http://localhost:5151
            // hacia HTTPS.
            // app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}