using System.ComponentModel.DataAnnotations;

namespace SpotifyApp.Api.ViewModels
{
    public class CopyPlaylistViewModel
    {
        [Required]
        [Display(Name = "Origin playlistId")]
        public string OriginPlaylistId { get; set; }

        [Required]
        [Display(Name = "Target playlistId")]
        public string TargetPlaylistId { get; set; }
    }
}
