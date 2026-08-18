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

            await builder.Build().RunAsync();
        }
    }
}