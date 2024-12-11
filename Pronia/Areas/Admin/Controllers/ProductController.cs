using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DataAccess;
using Pronia.Models;
using Pronia.ViewModel.ProductVM;
using System.Drawing;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        readonly ProniaDbContext _context;
        readonly IWebHostEnvironment _env;

        public ProductController(ProniaDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var datas = await _context.Products.Include(x => x.Category).ToListAsync();
            return View(datas);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories=await _context.Categories.Where(x=>!x.IsDeleted).ToListAsync();
            return View();
        }
        [HttpPost]         
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {
            ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            if(vm.CoverImage != null)
            {
                if (!vm.CoverImage.ContentType.StartsWith("image"))
                    ModelState.AddModelError("CoverImage", "File type must be image.");
                if (vm.CoverImage.Length > 600 * 1024)
                    ModelState.AddModelError("CoverImage", "File length cann't be long from 600kb.");
            }

            if (vm.OtherImages != null)
            {
                if (!vm.OtherImages.All(x => x.ContentType.StartsWith("image")))
                {
                    var fileNames = vm.OtherImages.Where(x => !x.ContentType.StartsWith("image")).Select(x => x.FileName);
                    ModelState.AddModelError("OtherImages",string.Join(", ", fileNames)+" is(are) not image.");
                }
                if(vm.OtherImages.Any(x=>x.Length>600 * 1024))
                {
                    var fileNames=vm.OtherImages.Where(x => x.Length > 600 * 1024).Select(x => x.FileName);
                    ModelState.AddModelError("OtherImages", string.Join(", ", fileNames) + " is(are) cann't be long 600kb.");
                }
            }
            if (!ModelState.IsValid)
                return View();

            string NewFilename = Path.GetRandomFileName() + Path.GetExtension(vm.CoverImage.FileName);
            using(Stream stream = System.IO.File.Create(Path.Combine(_env.WebRootPath, "imgs", "products", NewFilename)))
            {
                await vm.CoverImage.CopyToAsync(stream);
            }

            Product product = new Product()
            {
                CoverImage = NewFilename,
                CategoryId = vm.CategoryId,
                Name = vm.Name,
                Description = vm.Description,
                SellPrice = vm.SellPrice,
                CostPrice = vm.CostPrice,
                Quantity = vm.Quantity,
                Discount = vm.Discount
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();


            foreach (var item in vm.OtherImages)
            {
                string NewFileName=Path.GetRandomFileName()+Path.GetExtension(item.FileName);
                using(Stream stream = System.IO.File.Create(Path.Combine(_env.WebRootPath, "imgs", "products", NewFileName)))
                {
                    await item.CopyToAsync(stream);
                }

                ProductImage image = new ProductImage()
                {
                    FileUrl=NewFileName,
                    ProductId = product.Id
                };

                await _context.ProductImages.AddAsync(image);
                await _context.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update()
        {
            ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id,ProductCreateVM vm)
        {
            ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            if (vm.CoverImage != null)
            {
                if (!vm.CoverImage.ContentType.StartsWith("image"))
                    ModelState.AddModelError("CoverImage", "File type can only be image.");
                if (vm.CoverImage.Length > 600 * 1024)
                    ModelState.AddModelError("CoverImage", "File length cann't be long 600kb.");
            }
            if(!ModelState.IsValid)
                return View();

            var data=await _context.Products.FindAsync(id);
            string OldFilepath =Path.Combine(_env.WebRootPath, "imgs", "products",data.CoverImage);
            if (System.IO.File.Exists(OldFilepath))
            {
                System.IO.File.Delete(OldFilepath);
            }

            using(Stream stream = System.IO.File.Create(Path.Combine(_env.WebRootPath, "imgs", "products", OldFilepath)))
            {
                await vm.CoverImage.CopyToAsync(stream);
            }

            data.Quantity = vm.Quantity;
            data.Discount= vm.Discount;
            data.CategoryId = vm.CategoryId;
            data.SellPrice = vm.SellPrice;
            data.CostPrice = vm.CostPrice;
            data.Name= vm.Name;
            data.Description= vm.Description;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int Id)
        {
            var data = await _context.Products.Include(x=>x.images).FirstOrDefaultAsync(x=>x.Id==Id);

            if(data.images.Any())
            {
                foreach (var image in data.images)
                {
                    var filePath = Path.Combine(_env.WebRootPath, "imgs", "products", image.FileUrl);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.ProductImages.RemoveRange(data.images);
            }
            

            string Filepath = Path.Combine(_env.WebRootPath, "imgs", "products", data.CoverImage);
            if (System.IO.File.Exists(Filepath))
            {
                System.IO.File.Delete(Filepath);
            }

            _context.Products.Remove(data);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Hide(int id)
        {
            var data=await _context.Products.FindAsync(id);
            data.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Show(int id)
        {
            var data = await _context.Products.FindAsync(id);
            data.IsDeleted = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
