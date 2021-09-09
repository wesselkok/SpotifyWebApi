using SpotifyApp.Core.Base.Extensions;
using SpotifyApp.Core.Services.SpotifyAccountService.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpotifyApp.Core.Services.SpotifyAccountService
{
    public class SpotifyAccountService : ISpotifyAccountService
    {
        private readonly HttpClient _httpClient;

        public SpotifyAccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public string CreateAuthorizeUri(string clientId, string[] scopes = null)
        {
            // Build redirect uri
            NameValueCollection nvc = new NameValueCollection();
            nvc["client_id"] = clientId;
            nvc["response_type"] = "code";
            nvc["redirect_uri"] = "https://localhost:44372/callback";
            if (!scopes.IsNullOrEmpty()) nvc["scope"] = string.Join(" ", scopes);

            return $"https://accounts.spotify.com/authorize{nvc.ToQueryString()}";
        }

        public async Task<string> GetToken(string clientId, string clientSecret, string code)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");

            request.Headers.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}")));

            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", "https://localhost:44372/callback" }
            });

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            using var responeStream = await response.Content.ReadAsStreamAsync();
            var authResult = await JsonSerializer.DeserializeAsync<AuthResult>(responeStream);

            return authResult.access_token;
        }
    }
}
