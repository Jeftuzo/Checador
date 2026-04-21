using Microsoft.EntityFrameworkCore;
using Venta.API.Data;
using Venta.API.Services;

namespace Venta.API
{
    public static class DependencyContainer
    {
        public static IServiceCollection ConfigurandoDependencias(this IServiceCollection servicios, IConfiguration configuraciones)
        {

            var connectionString = configuraciones.GetConnectionString("DefaultConnection");

            servicios.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Add services to the container.

            servicios.AddControllers()
                        .AddJsonOptions(options =>
                        {
                            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                        });
            
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            servicios.AddOpenApi();

            servicios.AddScoped<ProductosServices>();
            servicios.AddScoped<ClientesServices>();
            servicios.AddScoped<VentasServices>();

            servicios.AddCors(options => //La seguridad. Previene ataques por DDOS. Permites los origenes y los metodos.
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });


            return servicios;
        }
    }
}
