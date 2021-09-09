using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyApp.Core.Services.SpotifyPlaylistService.Models.Response
{
    public class SpotifyPlaylistResult
    {
        public string href { get; set; }

        public List<SpotifyPlaylist> items { get; set; }

        public int? limit { get; set; }

        public string next { get; set; }

        public int? offset { get; set; }

        public string previous { get; set; }

        public int? total { get; set; }
    }
}
