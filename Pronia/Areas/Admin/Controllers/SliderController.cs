using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DataAccess;
using Pronia.Models;
using Pronia.ViewModel.SliderVM;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        readonly ProniaDbContext _context;
        readonly IWebHostEnvironment _env;

        public SliderController(ProniaDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Sliders.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(SliderCreateVM vm)
        {
            if (vm.File != null)
            {
                if (!vm.File.ContentType.StartsWith("image"))
                    ModelState.AddModelError("File", "File must be only image.");
                if (vm.File.Length > 600 * 1024)
                    ModelState.AddModelError("File", "File length must be less than 600kb.");

            }
            if (!ModelState.IsValid)
                return View();

            string NewFilename = Path.GetRandomFileName() + Path.GetExtension(vm.File.FileName);
            using (Stream stream = System.IO.File.Create(Path.Combine(_env.WebRootPath, "imgs", "sliders", NewFilename)))
            {
                await vm.File.CopyToAsync(stream);
            }

            Slider slider = new Slider()
            {
                ImageUrl = NewFilename,
                Title = vm.Title,
                Subtitle = vm.Subtitle,
                Link = vm.Link
            };

            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));  
        }
        public async Task<IActionResult> Update()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Update(int Id, SliderCreateVM vm)
        {
            if(vm.File != null)
            {
                if (!vm.File.ContentType.StartsWith("image"))
                    ModelState.AddModelError("File", "File type must be image.");
                if (vm.File.Length > 600 * 1024)
                    ModelState.AddModelError("File", "File length cann't be large than 600kb.");
            }
            if (!ModelState.IsValid)
                return View();

            var data = await _context.Sliders.FindAsync(Id);
            string OldFilepath=Path.Combine(_env.WebRootPath,"imgs","sliders",data.ImageUrl);
            if (System.IO.File.Exists(OldFilepath))
            {
                System.IO.File.Delete(OldFilepath);
            }

            using(Stream stream = System.IO.File.Create(Path.Combine(_env.WebRootPath, "imgs", "sliders", OldFilepath)))
            {
                await vm.File.CopyToAsync(stream);
            }

            data.Title = vm.Title;
            data.Subtitle = vm.Subtitle;
            data.Link = vm.Link;

            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var data= await _context.Sliders.FindAsync(id);
            string Filepath=Path.Combine(_env.WebRootPath,"imgs","sliders",data.ImageUrl);
            if (System.IO.File.Exists(Filepath))
            {
                System.IO.File.Delete(Filepath);
            }

            _context.Sliders.Remove(data);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Hide(int Id)
        {
            var data=await _context.Sliders.FindAsync(Id);
            data.IsDeleted= true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Show(int Id)
        {
            var data = await _context.Sliders.FindAsync(Id);
            data.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
