using Microsoft.AspNetCore.Mvc;
using SpotifyApp.Api.Attributes;
using SpotifyApp.Api.ViewModels;
using SpotifyApp.Core.Config;
using SpotifyApp.Core.Logic.Interfaces;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpotifyApp.Api.Controllers
{
    [Route("[controller]")]
    [TokenRequired]
    public class SpotifyController : Controller
    {
        private readonly ISpotifyPlaylistLogic _spotifyPlaylistLogic;
        private SpotifyConfiguration _spotifyConfiguration;

        public SpotifyController(ISpotifyPlaylistLogic spotifyPlaylistLogic)
        {
            _spotifyPlaylistLogic = spotifyPlaylistLogic;
            _spotifyConfiguration = SpotifyConfiguration.Instance;
        }

        [HttpGet]
        [Route("playlists")]
        public async Task<IActionResult> GetPlaylists()
        {
            try
            {
                // Do logic
                var playlists = await _spotifyPlaylistLogic.GetPlaylists(_spotifyConfiguration.Token);
                ViewData["Playlists"] = playlists.items;
                return View("Playlists");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("tracks")]
        public async Task<IActionResult> GetTracks(string playlistId)
        {
            try
            {
                // Do logic
                var playlistTracks = await _spotifyPlaylistLogic.GetPlaylistTracksMinified(_spotifyConfiguration.Token, playlistId);
                ViewData["Tracks"] = playlistTracks;
                ViewData["PlaylistId"] = playlistId;
                return View("Tracks");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("backup")]
        public async Task<IActionResult> Backup(string playlistId)
        {
            try
            {
                // Do logic
                var playlist = await _spotifyPlaylistLogic.GetPlaylist(_spotifyConfiguration.Token, playlistId);
                var playlistTracks = await _spotifyPlaylistLogic.GetPlaylistTracksMinified(_spotifyConfiguration.Token, playlistId);
                string fileName = $"backup-{playlist.name.Trim()}.json";

                FileInfo info = new FileInfo(fileName);
                if (!info.Exists)
                {
                    using (StreamWriter writer = info.CreateText())
                    {
                        await writer.WriteLineAsync(JsonSerializer.Serialize(playlistTracks));
                    }
                }

                // Create file stream & read block of bytes from stream
                byte[] bytes = null;
                using (FileStream fileStream = info.OpenRead())
                {
                    bytes = new byte[fileStream.Length];
                    fileStream.Read(bytes, 0, Convert.ToInt32(fileStream.Length));
                }

                return File(bytes, "application/json", fileName);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("copy")]
        public async Task<IActionResult> Copy(CopyPlaylistViewModel copyPlaylistViewModel)
        {
            try
            {
                // Do logic
                var result = await _spotifyPlaylistLogic.CopyPlaylist(_spotifyConfiguration.Token, copyPlaylistViewModel.OriginPlaylistId, copyPlaylistViewModel.TargetPlaylistId);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("tracks/duplicate")]
        public async Task<IActionResult> GetDuplicateTracks(string playlistId)
        {
            try
            {
                // Do logic
                var playlistDuplicateTracks = await _spotifyPlaylistLogic.GetPlaylistDuplicateTracks(_spotifyConfiguration.Token, playlistId);
                ViewData["DuplicateTracks"] = playlistDuplicateTracks;
                return View("DuplicateTracks");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
