using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyApp.Core.Services.SpotifyPlaylistService.Models
{
    public class SpotifyAlbum
    {
        public string album_type { get; set; }
        public string[] available_markets { get; set; }
        public SpotifyExternalUrls external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public SpotifyImage[] images { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }
}
