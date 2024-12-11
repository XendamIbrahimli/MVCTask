using Pronia.Models;
using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModel.ProductVM
{
    public class ProductCreateVM
    {
        [Required,MaxLength(20,ErrorMessage ="Name cann't be long from 20 characters.")]
        public string Name { get; set; } = null!;

        [Required, MaxLength(60, ErrorMessage = "Description cann't be long from 60 characters.")]
        public string Description { get; set; } = null!;
        public decimal CostPrice { get; set; }
        public decimal SellPrice { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }
        public IFormFile CoverImage { get; set; }
        public ICollection<IFormFile>? OtherImages { get; set; }
        public int? CategoryId { get; set; }
    }
}
