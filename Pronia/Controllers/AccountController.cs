using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pronia.Models;
using Pronia.ViewModel.Auths;

namespace Pronia.Controllers
{
    public class AccountController(UserManager<User> _userManager) : Controller
    {
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterCreateVM vm)
        {
            if(!ModelState.IsValid)
                return View();

            User user = new()
            {
                UserName=vm.Username,
                FullName=vm.Fullname,
                Email=vm.Email,
                ProfileImageUrl="foto.jpg"
            };

            var result=await _userManager.CreateAsync(user,vm.Password);
            if (!result.Succeeded)
            {
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError("",item.Description);
                }
                return View();
            }


            return View();
        }
    }
}
