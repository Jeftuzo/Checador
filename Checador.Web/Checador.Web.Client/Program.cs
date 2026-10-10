using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Checador.Web.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            var apiBaseUrl =
                builder.Configuration["ApiBaseUrl"]
                ?? "http://localhost:5151/";

            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(apiBaseUrl)
            });

            builder.Services.AddScoped<Checador.Web.Client.Services.AuthState>();

            await builder.Build().RunAsync();
        }
    }
}