using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpotifyApp.Core.Logic;
using SpotifyApp.Core.Logic.Interfaces;
using SpotifyApp.Core.Services.SpotifyAccountService;
using SpotifyApp.Core.Services.SpotifyPlaylistService;

namespace SpotifyApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, _services) =>
                {
                    _services.AddSingleton<ISpotifyPlaylistLogic, SpotifyPlaylistLogic>();
                    _services.AddSingleton<ISpotifyAccountService, SpotifyAccountService>();
                    _services.AddSingleton<ISpotifyPlaylistService, SpotifyPlaylistService>();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
