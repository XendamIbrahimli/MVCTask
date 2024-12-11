namespace Pronia.ViewModel.ProductVM
{
    public class ProductItemVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public string ImageUrl { get; set; }
        public bool IsInStock { get; set; }
    }
}
