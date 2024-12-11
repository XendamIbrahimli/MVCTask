using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModel.SliderVM
{
    public class SliderCreateVM
    {
        [Required(ErrorMessage ="Title is required"), MaxLength(32,ErrorMessage ="Title must be less than 32")]
        public string Title { get; set; }
        [Required,MaxLength(64)]
        public string Subtitle { get; set; }
        public string? Link { get; set; }
        [Required]
        public IFormFile File { get; set; }
    }
}
