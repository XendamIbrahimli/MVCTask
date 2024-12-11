using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DataAccess;
using Pronia.ViewModel.Common;
using Pronia.ViewModel.ProductVM;
using Pronia.ViewModel.SliderVM;

namespace Pronia.Controllers
{
    public class HomeController : Controller
    {
        readonly ProniaDbContext _context;

        public HomeController(ProniaDbContext context)
        {
            _context = context;
        } 

        public async Task<IActionResult> Index()
        {
            HomeVM vm = new();
            vm.Sliders= await _context.Sliders.Where(x => !x.IsDeleted).Select(x => new SliderItemVM
            {
                ImageUrl = x.ImageUrl,
                Title = x.Title,
                Subtitle = x.Subtitle,
                Link = x.Link
            }).ToListAsync();

            vm.Products = await _context.Products.Where(x => !x.IsDeleted).Select(x => new ProductItemVM
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.SellPrice,
                ImageUrl = x.CoverImage,
                IsInStock = x.Quantity > 0,
                Discount = x.Discount
            }).ToListAsync();
            return View(vm);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }
    }
}

