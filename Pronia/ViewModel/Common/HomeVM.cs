using Pronia.ViewModel.ProductVM;
using Pronia.ViewModel.SliderVM;

namespace Pronia.ViewModel.Common
{
    public class HomeVM
    {
        public IEnumerable<SliderItemVM> Sliders { get; set; }
        public IEnumerable<ProductItemVM> Products { get; set; }
    }
}
