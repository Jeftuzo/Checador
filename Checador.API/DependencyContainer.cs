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
            servicios.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(SqlServerConnection));

            //PostgreSQL 17
            //var postgresConnection = configuraciones.GetConnectionString("PostgreSQLConnection");
            //servicios.AddDbContext<ApplicationDbContext>(options =>
            //   options.UseNpgsql(postgresConnection));


            servicios.AddScoped<Checador.API.Services.AsistenciaService>();
            servicios.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                options.SerializerOptions.PropertyNamingPolicy = null;
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

            servicios.AddScoped(sp =>
            {
                var clientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var client = clientFactory.CreateClient();
                client.BaseAddress = new Uri("https://localhost:7246/");
                return client;
            });
            return servicios;
        }
    }
}
