using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DataAccess;
using Pronia.Models;
using Pronia.ViewModel.CategoryVM;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        readonly ProniaDbContext db;

        public CategoryController(ProniaDbContext db)
        {
            this.db = db;
        }

        public async Task<IActionResult> Index()
        {
            return View(await db.Categories.Include(x=>x.Products).ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateVM vm)
        {
            if (!ModelState.IsValid)
                return View();

            Category category = new()
            {
                Name = vm.Name
            };
            await db.Categories.AddAsync(category);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            bool IsReferenced=await db.Products.AnyAsync(x=>x.CategoryId==id);
            if (IsReferenced)
            {
                TempData["Error"] = "You cann't delete this category because it referenced by products.";
                return RedirectToAction(nameof(Index));
            }

            var data=await db.Categories.FindAsync(id);

            db.Categories.Remove(data);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Update(CategoryCreateVM vm, int Id)
        {
            if (!ModelState.IsValid)
                return View();

            var data=await db.Categories.FindAsync(Id);

            data.Name=vm.Name;
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
