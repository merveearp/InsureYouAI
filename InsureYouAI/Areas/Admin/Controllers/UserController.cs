using InsureYouAI.Context;
using InsureYouAI.Entities;
using InsureYouAI.Services.OpenAIServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly InsureContext _context;
        private readonly IOpenAIService _openAIService;

        public UserController(UserManager<AppUser> userManager, InsureContext context, IOpenAIService openAIService)
        {
            _userManager = userManager;
            _context = context;
            _openAIService = openAIService;
        }

        public IActionResult UserList()
        {
            var values = _userManager.Users.ToList();
            return View(values);
        }
        public async Task<IActionResult> UserProfileWithAI(string id)
        {
            var value = _userManager.Users.Where(x=>x.Id==id).FirstOrDefault();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı!");


            var articles = await _context.Articles
                .Where(x => x.AppUserId == id)
                .Select(y => y.Content)
                .ToListAsync();

            ViewBag.Count = articles.Count;

            if(articles.Count==0)
            {
                ViewBag.AIResult = "Bu kulllanıcıya ait analiz yapılacak makale bulunamadı!";
                return View(user);
            }

            var allArticles = string.Join("\n\n", articles);

            var AIResponse = await _openAIService.AnalyzeUserAsync(allArticles);

            ViewBag.AIResult = AIResponse;

            return View(value);
        }

        public async Task<IActionResult> UserCommentsProfileWithAI(string id)
        {
            var value = _userManager.Users.Where(x => x.Id == id).FirstOrDefault();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı!");
            var articles = await _context.Articles
               .Where(x => x.AppUserId == id)
               .Select(y => y.Content)
               .ToListAsync();

            ViewBag.Count = articles.Count;

            var comments = await _context.Comments
                .Where(x => x.AppUserId == id)
                .Select(y => y.CommentDetail)
                .ToListAsync();

            if (comments.Count == 0)
            {
                ViewBag.AIResult = "Bu kulllanıcıya ait analiz yapılacak yorum bulunamadı!";
                return View(user);
            }

            var allComments = string.Join("\n\n", comments);

            var AIResponse = await _openAIService.AnalyzeCommentUserAsync(allComments);

            ViewBag.AIResult = AIResponse;

            return View(value);
        }


    }
}
