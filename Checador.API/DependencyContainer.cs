using Microsoft.EntityFrameworkCore;
using Checador.API.Data;

namespace Checador.API
{
    public static class DependencyContainer
    {
        public static IServiceCollection ConfigurandoDependencias(this IServiceCollection servicios, IConfiguration configuraciones)
        {
            //SQL Server
            var SqlServerConnection = configuraciones.GetConnectionString("SqlServerConnection");

            if (string.IsNullOrEmpty(SqlServerConnection))
            {
                throw new Exception("No se encontró la cadena de conexión 'SqlServerConnection' en el appsettings.json.");
            }
            servicios.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(SqlServerConnection));

            //PostgreSQL 17
            //var postgresConnection = configuraciones.GetConnectionString("PostgreSQLConnection");
            //servicios.AddDbContext<ApplicationDbContext>(options =>
            //   options.UseNpgsql(postgresConnection));


            servicios.AddScoped<Checador.API.Services.AsistenciaService>();

            servicios.AddControllers()
                                 .AddJsonOptions(options =>
                                 {
                                     options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                                     options.JsonSerializerOptions.PropertyNamingPolicy = null;
                                 });

            servicios.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 104857600;
            });

            servicios.AddCors(options => 
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            servicios.AddHttpClient();

            return servicios;
        }
    }
}
