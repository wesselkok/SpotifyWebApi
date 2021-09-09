using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyApp.Core.Services.SpotifyPlaylistService.Models
{
    public class SpotifyPlaylist
    {
        public bool collaborative { get; set; }
        public string description { get; set; }
        public SpotifyExternalUrls external_urls { get; set; }
        public SpotifyFollowers followers { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public SpotifyImage[] images { get; set; }
        public string name { get; set; }
        public SpotifyOwner owner { get; set; }
        public object _public { get; set; }
        public string snapshot_id { get; set; }
        public SpotifyTracks tracks { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }
}
