using Microsoft.AspNetCore.Mvc;
using SpotifyApp.Core.Config;
using System.Threading.Tasks;

namespace SpotifyApp.Api.Controllers
{
    [Route("[controller]")]
    public class CallbackController : Controller
    {
        private SpotifyConfiguration _spotifyConfiguration;

        public CallbackController()
        {
            _spotifyConfiguration = SpotifyConfiguration.Instance;
        }

        public async Task<IActionResult> Callback(string code, string state = null)
        {
            return await Task.Run<IActionResult>(() =>
            {
                // Set code
                _spotifyConfiguration.Code = code;

                return RedirectToAction("Index", "Home");
            });
        }
    }
}
