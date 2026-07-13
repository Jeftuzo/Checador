using Microsoft.EntityFrameworkCore;
using Checador.API;

namespace Checador.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();

            builder.Services.ConfigurandoDependencias(builder.Configuration);

            var app = builder.Build();

            app.UseRouting();
            app.UseCors("AllowAll");

            if (app.Environment.IsDevelopment())
            {

            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
