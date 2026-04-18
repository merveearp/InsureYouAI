using InsureYouAI.Repositories.ArticleRepositories;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Controllers
{
    public class BlogController : Controller
    {
        private readonly IArticleRepository _articleRepository;

        public BlogController(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _articleRepository.GetAllAsync();
            return View(values);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var value = await _articleRepository.GetByIdAsync(id);
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


    }
}
