using InsureYouAI.Context;
using InsureYouAI.Entities;
using InsureYouAI.Repositories.ArticleRepositories;
using InsureYouAI.Services.HuggingFaceServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAI.Controllers
{
    public class BlogController : Controller
    {
        private readonly IArticleRepository _articleRepository;
        private readonly InsureContext _context;
        private readonly IHuggingFaceService _huggingFaceService;


        public BlogController(IArticleRepository articleRepository, InsureContext context, IHuggingFaceService huggingFaceService)
        {
            _articleRepository = articleRepository;
            _context = context;
            _huggingFaceService = huggingFaceService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 3;

            var values = await _articleRepository.GetAllAsync();

            var totalCount = values.Count();

            var pagedData = values
                .OrderByDescending(x => x.ArticleId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = Math.Max(1, (int)Math.Ceiling((double)totalCount / pageSize));

            return View(pagedData);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var value = _context.Articles
                .Include(x => x.Category)
                .Include(x => x.Comments.Where(x=>x.CommentStatus=="Yorum Onaylandı"))
                    .ThenInclude(x => x.AppUser)
                .FirstOrDefault(x => x.ArticleId == id);
            
            value.ViewCount++;
            _context.SaveChanges();

            return View(value);
        }

        public PartialViewResult GetBlog()
        {
            return PartialView();
        }

        [HttpPost]
        public IActionResult GetBlog(string keyword)
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult AddComment()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(Comment comment)
        {

            comment.CommentDate = DateTime.Now;
            comment.AppUserId = "54a60ec7-226c-4db0-b79f-c7dc14f3f41a";
            try
            {
                var englishText = await _huggingFaceService.GetTranslateText(comment);
                var score = await _huggingFaceService.GetToxicScore(englishText);

                comment.CommentStatus = score > 0.15
                    ? "Toksik Yorum"
                    : "Yorum Onaylandı";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Yorum analiz hatası: " + ex.Message);
                comment.CommentStatus = "Onay Bekliyor";
            }


            _context.Comments.Add(comment);
            _context.SaveChanges();
            return RedirectToAction("Detail", new { id = comment.ArticleId });

        }


    }
}
