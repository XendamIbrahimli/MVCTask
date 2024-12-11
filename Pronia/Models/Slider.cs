using System.ComponentModel.DataAnnotations;

namespace Pronia.Models
{
    public class Slider : BaseEntity
    {
        [MaxLength(32)]
        [Required]
        public string Title { get; set; }

        [MaxLength(64)]
        [Required]
        public string Subtitle { get; set; }
        public string? Link { get; set; }
        public string ImageUrl { get; set; }
    }
}
