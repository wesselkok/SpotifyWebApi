using SpotifyApp.Core.Logic.Interfaces;
using SpotifyApp.Core.Services.SpotifyPlaylistService;
using SpotifyApp.Core.Services.SpotifyPlaylistService.Models;
using SpotifyApp.Core.Services.SpotifyPlaylistService.Models.Response;
using System.Threading.Tasks;
using System.Linq;
using SpotifyApp.Core.Base.Extensions;
using System.Collections.Generic;
using System;

namespace SpotifyApp.Core.Logic
{
    public class SpotifyPlaylistLogic : ISpotifyPlaylistLogic
    {
        private readonly ISpotifyPlaylistService _spotifyPlaylistService;

        public SpotifyPlaylistLogic(ISpotifyPlaylistService spotifyPlaylistService)
        {
            _spotifyPlaylistService = spotifyPlaylistService;
        }

        public async Task<SpotifyPlaylistResult> GetPlaylists(string token) => await _spotifyPlaylistService.GetPlaylists(token);

        public async Task<SpotifyPlaylist> GetPlaylist(string token, string playlistId) => await _spotifyPlaylistService.GetPlaylist(token, playlistId);

        public async Task<List<SpotifyTrackModel>> GetPlaylistTracksMinified(string token, string playlistId)
        {
            var tracks = await GetPlaylistTracks(token, playlistId);
            return tracks.Select(x => new SpotifyTrackModel { Id = x.id, Name = x.name, Image = x.album?.images.FirstOrDefault()?.url, Link = x.external_urls.spotify }).ToList();
        }

        public async Task<List<SpotifyTrack>> GetPlaylistTracks(string token, string playlistId)
        {
            // Get playlist tracks
            List<SpotifyTracks> spotifyTracks = new List<SpotifyTracks>();
            var tracks = await GetPlaylistTracks(token, playlistId, limit: 100, offset: 0);
            spotifyTracks.Add(tracks);
            if (tracks.total > tracks.items.Length)
            {
                // Loop items
                for (int offset = 100; offset < tracks.total; offset += 100)
                {
                    spotifyTracks.Add(await GetPlaylistTracks(token, playlistId, limit: 100, offset: offset));
                }
            }

            // Select all spotify track names & id
            var spotifyTracksModel = new List<SpotifyTrack>();
            spotifyTracks.Select(x => x.items.Where(z => !z.is_local)).ForEach(y => y.ForEach(z => spotifyTracksModel.Add(z.track)));
            return spotifyTracksModel;
        }

        public async Task<SpotifyTracks> GetPlaylistTracks(string token, string playlistId, int? limit = null, int? offset = null)
        {
            return await _spotifyPlaylistService.GetPlaylistTracks(token, playlistId, limit: limit, offset: offset);
        }

        public async Task<List<SpotifyDuplicateResult>> GetPlaylistDuplicateTracks(string token, string playlistId)
        {
            // Get tracks
            var tracks = await GetPlaylistTracksMinified(token, playlistId);

            // Query
            var result = tracks.GroupBy(x => x.Name)
              .Where(g => g.Count() > 1)
              .Select(y => new SpotifyDuplicateResult { Name = y.Key, Count = y.Count(), Track = tracks.FirstOrDefault(z => z.Name == y.Key) })
              .ToList();

            // Find duplicates
            return result;
        }

        public async Task<bool> DeletePlaylistTrack(string token, string playlistId, SpotifyTrack track)
        {
            if (track == null) throw new ArgumentNullException(nameof(track));
            return await DeletePlaylistTracks(token, playlistId, new List<SpotifyTrack>() { track });
        }

        public async Task<bool> DeletePlaylistTracks(string token, string playlistId, List<SpotifyTrack> tracks)
        {
            return await _spotifyPlaylistService.DeletePlaylistTracks(token, playlistId, tracks);
        }

        public async Task<bool> AddPlaylistTrack(string token, string playlistId, SpotifyTrack track)
        {
            if (track == null) throw new ArgumentNullException(nameof(track));
            return await AddPlaylistTracks(token, playlistId, new List<SpotifyTrack>() { track });
        }

        public async Task<bool> AddPlaylistTracks(string token, string playlistId, List<SpotifyTrack> tracks)
        {
            return await _spotifyPlaylistService.AddPlaylistTracks(token, playlistId, tracks);
        }

        public async Task<bool> CopyPlaylist(string token, string originplaylistId, string targetPlaylistId)
        {
            // Delete tracks from target playlist
            var targetTracks = await GetPlaylistTracks(token, targetPlaylistId);
            if (targetTracks.Any())
            {
                var deleteResult = await DeletePlaylistTracks(token, targetPlaylistId, targetTracks);
                if (!deleteResult) throw new Exception("Delete result is false");
            }

            // Insert tracks to target playlists
            var originTracks = await GetPlaylistTracks(token, originplaylistId);
            if (!originTracks.Any()) throw new Exception("No tracks in origin playlist");
            var addResult = await AddPlaylistTracks(token, targetPlaylistId, originTracks);
            if (!addResult) throw new Exception("Add result is false");

            return true;
        }
    }
}
