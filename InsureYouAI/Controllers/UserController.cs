using InsureYouAI.DTOs.UserDtos;
using InsureYouAI.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(RegisterDto registerUser)
        {
            AppUser appUser = new AppUser()
            {
                Name = registerUser.Name,
                Surname = registerUser.Surname,
                Email = registerUser.Email,
                UserName = registerUser.UserName,
                ImageUrl = "/images/default-user.png",
                Description = "Unvan alanı henüz güncellenmemiştir"


            };

            await _userManager.CreateAsync(appUser,registerUser.Password);
            return RedirectToAction("/User/Login");

        }

    }
}
