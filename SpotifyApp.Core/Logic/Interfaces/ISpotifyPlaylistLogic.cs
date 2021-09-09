using SpotifyApp.Core.Services.SpotifyPlaylistService.Models;
using SpotifyApp.Core.Services.SpotifyPlaylistService.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotifyApp.Core.Logic.Interfaces
{
    public interface ISpotifyPlaylistLogic
    {
        Task<SpotifyPlaylistResult> GetPlaylists(string token);
        Task<SpotifyPlaylist> GetPlaylist(string token, string playlistId);
        Task<List<SpotifyTrackModel>> GetPlaylistTracksMinified(string token, string playlistId);
        Task<List<SpotifyTrack>> GetPlaylistTracks(string token, string playlistId);
        Task<SpotifyTracks> GetPlaylistTracks(string token, string playlistId, int? limit = null, int? offset = null);
        Task<List<SpotifyDuplicateResult>> GetPlaylistDuplicateTracks(string token, string playlistId);
        Task<bool> DeletePlaylistTrack(string token, string playlistId, SpotifyTrack track);
        Task<bool> DeletePlaylistTracks(string token, string playlistId, List<SpotifyTrack> tracks);
        Task<bool> AddPlaylistTrack(string token, string playlistId, SpotifyTrack track);
        Task<bool> AddPlaylistTracks(string token, string playlistId, List<SpotifyTrack> tracks);
        Task<bool> CopyPlaylist(string token, string originplaylistId, string targetPlaylistId);
    }
}
