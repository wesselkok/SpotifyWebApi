using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyApp.Core.Services.SpotifyPlaylistService.Models
{
    public class SpotifyItem
    {
        public DateTime added_at { get; set; }
        public SpotifyOwner added_by { get; set; }
        public bool is_local { get; set; }
        public SpotifyTrack track { get; set; }
    }
}
