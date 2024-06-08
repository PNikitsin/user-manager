using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManager.Entities;
using UserManager.ViewModels;

namespace UserManager.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public AdminController(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            var response = new UserListModel
            {
                Users = _mapper.Map<IEnumerable<UserViewModel>>(users)
            };

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Block(UserListModel model)
        {
            if (model.UserIds == null) return RedirectToAction("Index");

            var users = _userManager.Users.Where(user => model.UserIds.Contains(user.Id)).ToList();
            var currentUser = users.FirstOrDefault(user => user.UserName == GetUsername());

            if (currentUser != null)
            {
                await _signInManager.SignOutAsync();
            }

            foreach (var user in users)
            {
                user.IsBlocked = true;
                user.LockoutEnd = DateTimeOffset.MaxValue;

                await _userManager.UpdateAsync(user);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Unblock(UserListModel model)
        {
            if (model.UserIds == null) return RedirectToAction("Index");

            var users = _userManager.Users.Where(user => model.UserIds.Contains(user.Id)).ToList();
            var currentUser = users.FirstOrDefault(user => user.UserName == GetUsername());

            foreach (var user in users)
            {
                user.IsBlocked = false;
                user.LockoutEnd = DateTime.Now;

                await _userManager.UpdateAsync(user);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UserListModel model)
        {
            if (model.UserIds == null) return RedirectToAction("Index");

            var users = _userManager.Users.Where(user => model.UserIds.Contains(user.Id)).ToList();
            var currentUser = users.FirstOrDefault(user => user.UserName == GetUsername());

            if (currentUser != null)
            {
                await _signInManager.SignOutAsync();
            }

            foreach (var user in users) 
            {
                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                }
            }

            return RedirectToAction("Index");
        }

        public string GetUsername()
        {
            return User.Identity.Name;
        }
    }
}