using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManager.Entities;
using UserManager.ViewModels;

namespace UserManager.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid) return View(registerViewModel);

            var user = await _userManager.FindByEmailAsync(registerViewModel.Email);

            if (user != null)
            {
                ModelState.AddModelError(string.Empty, "This email is already taken.");

                return View(registerViewModel);
            }

            user = new ApplicationUser
            {
                Email = registerViewModel.Email,
                UserName = registerViewModel.Email,
                CreatedAt = DateTime.UtcNow
            };

            var newUserResponse = await _userManager.CreateAsync(user, registerViewModel.Password);

            if (newUserResponse.Succeeded)
            {
                user.LoginedAt = DateTime.UtcNow;

                await _signInManager.SignInAsync(user, isPersistent: false);
                await _userManager.UpdateAsync(user);

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in newUserResponse.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(registerViewModel);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);

            var user = await _userManager.FindByEmailAsync(loginViewModel.Email);

            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);

                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);

                    if (result.Succeeded)
                    {
                        user.LoginedAt = DateTime.UtcNow;
                        await _userManager.UpdateAsync(user);

                        return RedirectToAction("Index", "Home");
                    }
                    if (result.IsLockedOut)
                    {
                        ModelState.AddModelError(string.Empty, "Account is blocked.");

                        return View(loginViewModel);
                    }
                }

                ModelState.AddModelError(string.Empty, "Incorrect login and (or) password.");

                return View(loginViewModel);
            }

            ModelState.AddModelError(string.Empty, "Incorrect login and (or) password.");

            return View(loginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
} 