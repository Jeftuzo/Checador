using Microsoft.AspNetCore.Mvc;
using Venta.API.Data;
using Venta.API.Data.Entities;

namespace Venta.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public WeatherForecastController(ApplicationDbContext context)
        {
            _context = context;
        }

        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var producto = new Producto
            {
                Nombre = "Producto de prueba 1",
                Precio = 9.99m
            };
            _context.Productos.Add(producto);
            _context.SaveChanges();

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
