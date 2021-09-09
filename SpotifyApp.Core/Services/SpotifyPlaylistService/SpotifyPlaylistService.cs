using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json.Linq;
using SpotifyApp.Core.Base.Extensions;
using SpotifyApp.Core.Services.SpotifyPlaylistService.Models;
using SpotifyApp.Core.Services.SpotifyPlaylistService.Models.Response;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpotifyApp.Core.Services.SpotifyPlaylistService
{
    public class SpotifyPlaylistService : ISpotifyPlaylistService
    {
        
        private readonly HttpClient _httpClient;

        public SpotifyPlaylistService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<SpotifyPlaylistResult> GetPlaylists(string token)
        {
            // Check parameters
            if (string.IsNullOrWhiteSpace(token)) throw new Exception("Token can not be null");

            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.spotify.com/v1/me/playlists");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            using var responeStream = await response.Content.ReadAsStreamAsync();
            var playlists = await JsonSerializer.DeserializeAsync<SpotifyPlaylistResult>(responeStream);

            return playlists;
        }

        public async Task<SpotifyPlaylist> GetPlaylist(string token, string playlistId)
        {
            // Check parameters
            if (string.IsNullOrWhiteSpace(token)) throw new Exception("Token can not be null");
            if (string.IsNullOrWhiteSpace(playlistId)) throw new Exception("PlaylistId can not be null");

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.spotify.com/v1/playlists/{playlistId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            using var responeStream = await response.Content.ReadAsStreamAsync();
            var playlists = await JsonSerializer.DeserializeAsync<SpotifyPlaylist>(responeStream);

            return playlists;
        }


        public async Task<SpotifyTracks> GetPlaylistTracks(string token, string playlistId, int? limit = null, int? offset = null)
        {
            // Check parameters
            if (string.IsNullOrWhiteSpace(token)) throw new Exception("Token can not be null");

            // Query parameters
            var queryString = new Dictionary<string, string>();
            if (limit != null) queryString.Add("limit", limit.GetValueOrDefault().ToString());
            if (offset != null) queryString.Add("offset", offset.GetValueOrDefault().ToString());

            var request = new HttpRequestMessage(HttpMethod.Get, QueryHelpers.AddQueryString($"https://api.spotify.com/v1/playlists/{playlistId}/tracks", queryString));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            using var responeStream = await response.Content.ReadAsStreamAsync();
            var tracks = await JsonSerializer.DeserializeAsync<SpotifyTracks>(responeStream);

            return tracks;
        }

        public async Task<bool> AddPlaylistTracks(string token, string playlistId, List<SpotifyTrack> tracks)
        {
            // Check parameters
            if (string.IsNullOrWhiteSpace(token)) throw new Exception("Token can not be null");

            for (int i = 0; i < tracks.Count; i += 100)
            {
                // Query parameters
                var queryString = new Dictionary<string, string>();

                var request = new HttpRequestMessage(HttpMethod.Post, QueryHelpers.AddQueryString($"https://api.spotify.com/v1/playlists/{playlistId}/tracks", queryString));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                JObject jObject = new JObject();
                JArray jArray = new JArray();
                tracks.Skip(i).Take(100).ForEach(x =>
                {
                    jArray.Add(x.uri);
                });

                jObject["uris"] = jArray;

                request.Content = new StringContent(jObject.ToString());

                var response = await _httpClient.SendAsync(request);

                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> DeletePlaylistTracks(string token, string playlistId, List<SpotifyTrack> tracks)
        {
            // Check parameters
            if (string.IsNullOrWhiteSpace(token)) throw new Exception("Token can not be null");

            for (int i = 0; i < tracks.Count; i += 100)
            {

                // Query parameters
                var queryString = new Dictionary<string, string>();

                var request = new HttpRequestMessage(HttpMethod.Delete, QueryHelpers.AddQueryString($"https://api.spotify.com/v1/playlists/{playlistId}/tracks", queryString));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                JObject jObject = new JObject();
                JArray jArray = new JArray();
                tracks.Skip(i).Take(100).ForEach(x =>
                {
                    jArray.Add(new JObject() { { "uri", x.uri } });
                });

                jObject["tracks"] = jArray;

                request.Content = new StringContent(jObject.ToString());

                var response = await _httpClient.SendAsync(request);

                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    return false;
                }
            }

            return true;
        }
    }
}
