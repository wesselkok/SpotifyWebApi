using System.Threading.Tasks;

namespace SpotifyApp.Core.Services.SpotifyAccountService
{
    public interface ISpotifyAccountService
    {
        string CreateAuthorizeUri(string clientId, string[] scopes = null);
        Task<string> GetToken(string clientId, string clientSecret, string code);
    }
}
