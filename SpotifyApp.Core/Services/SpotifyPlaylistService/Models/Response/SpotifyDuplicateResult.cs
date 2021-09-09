
namespace SpotifyApp.Core.Services.SpotifyPlaylistService.Models.Response
{
    public class SpotifyDuplicateResult
    { 
        public string Name { get; set; }
        public int Count { get; set; }

        public SpotifyTrackModel Track { get; set; }
    }
}
