using InsureYouAI.DTOs.UserDtos;
using InsureYouAI.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace InsureYouAI.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
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
        public async Task<IActionResult> Register(RegisterDto registerUser)
        {



            if (!registerUser.AcceptTerms)
            {
                ModelState.AddModelError("AcceptTerms", "Sözleşmeyi kabul etmelisiniz.");
                return View(registerUser);
            }


            if (!ModelState.IsValid)
            {
                return View(registerUser);
            }



            var user = await _userManager.FindByEmailAsync(registerUser.Email);

            if (user != null)
            {
                ModelState.AddModelError("", "Bu email ile kayıtlı kullanıcı mevcut.");
                return View(registerUser);
            }

            var appUser = new AppUser()
            {
                Name = registerUser.Name,
                Surname = registerUser.Surname,
                Email = registerUser.Email.ToLower(),
                UserName = registerUser.Email.ToLower(),
                ImageUrl = "/images/default-user.png",
                Description = "Unvan alanı henüz güncellenmemiştir"
                
            };

       
            var result = await _userManager.CreateAsync(appUser, registerUser.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(registerUser);
            }


            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var appUser = await _userManager.FindByEmailAsync(loginDto.Email);

            if (appUser == null)
            {
                return BadRequest("Email veya şifre hatalı.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginDto.Password, true);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    return BadRequest("Çok fazla hatalı giriş yaptınız.");
                }

                return BadRequest("Email veya şifre hatalı.");
            }

            await _signInManager.SignInAsync(appUser, false);

            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }


    }
}
