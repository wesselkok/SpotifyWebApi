using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SpotifyApp.Core.Config;
using SpotifyApp.Core.Logic.Interfaces;
using SpotifyApp.Core.Services.SpotifyAccountService;
using SpotifyApp.Core.Services.SpotifyPlaylistService;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SpotifyApp.Api.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ISpotifyAccountService _spotifyAccountService;
        private readonly IConfiguration _configuration;
        private SpotifyConfiguration _spotifyConfiguration;
        private readonly string[] scopes = new string[] { "playlist-read-collaborative", "playlist-modify-public", "playlist-read-private", "playlist-modify-private" };

        public HomeController(ISpotifyAccountService spotifyAccountService, ISpotifyPlaylistService spotifyPlaylistService, IConfiguration configuration)
        {
            _spotifyAccountService = spotifyAccountService;
            _configuration = configuration;
            _spotifyConfiguration = SpotifyConfiguration.Instance;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            try
            {
                // Check token
                if (string.IsNullOrWhiteSpace(_spotifyConfiguration.Token))
                {
                    if (string.IsNullOrWhiteSpace(_spotifyConfiguration.Code)) return Redirect(_spotifyAccountService.CreateAuthorizeUri(_configuration["Spotify:ClientId"], scopes: scopes));
                    var token = await _spotifyAccountService.GetToken(_configuration["Spotify:ClientId"], _configuration["Spotify:ClientSecret"], _spotifyConfiguration.Code);
                    if (string.IsNullOrWhiteSpace(token)) throw new Exception("Token can not be null");

                    // Set token
                    _spotifyConfiguration.Token = token;
                }

                if (string.IsNullOrWhiteSpace(_spotifyConfiguration.Token)) throw new Exception("Invalid token");
                return View();

            }
            catch (Exception ex)
            {
                Debug.Write(ex);
                return BadRequest();
            }   
        }
    }
}
