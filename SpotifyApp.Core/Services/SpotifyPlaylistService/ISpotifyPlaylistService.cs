using SpotifyApp.Core.Services.SpotifyPlaylistService.Models;
using SpotifyApp.Core.Services.SpotifyPlaylistService.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotifyApp.Core.Services.SpotifyPlaylistService
{
    public interface ISpotifyPlaylistService
    {
        Task<SpotifyPlaylistResult> GetPlaylists(string token);
        Task<SpotifyPlaylist> GetPlaylist(string token, string playlistId);
        Task<SpotifyTracks> GetPlaylistTracks(string token, string playlistId, int? limit = null, int? offset = null);
        Task<bool> AddPlaylistTracks(string token, string playlistId, List<SpotifyTrack> tracks);
        Task<bool> DeletePlaylistTracks(string token, string playlistId, List<SpotifyTrack> tracks);
    }
}
