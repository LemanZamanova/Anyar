using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _sigInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> sigInManager)
        {
            _userManager = userManager;
            _sigInManager = sigInManager;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {

            {
                if (!ModelState.IsValid) return View();
                AppUser user = new AppUser
                {
                    Name = registerVM.Name,
                    Surname = registerVM.Surname,
                    UserName = registerVM.UserName,
                    Email = registerVM.Email,

                };
                var result = await _userManager.CreateAsync(user, registerVM.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();

                }
                await _sigInManager.SignInAsync(user, false);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }





        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginVM.UserNameOrEmail || u.Email == loginVM.UserNameOrEmail);

            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "username ,email or password is incorrect");
                return View();
            }
            var result = await _sigInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.IsPersistent, true);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "username ,email or password is incorrect");
                return View();
            }

            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> Logout()
        {
            await _sigInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
