using InsureYouAI.Entities;
using InsureYouAI.Repositories.ArticleRepositories;
using InsureYouAI.Repositories.CategoryRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InsureYouAI.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ArticleController : Controller
    {
        private readonly IArticleRepository _repository;
        private readonly ICategoryRepository _categoryRepository;
       

        public ArticleController(IArticleRepository repository, ICategoryRepository categoryRepository)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
        }

        private async Task GetCategories()
        {
            var categoryList = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = (from category in categoryList
                                select new SelectListItem
                                {
                                    Text = category.CategoryName,
                                    Value = category.CategoryId.ToString()
                                });
        }

        public async Task<IActionResult> ArticleList()
        {
            var values = await _repository.GetAllAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Article article)
        {
            await GetCategories();
            await _repository.CreateAsync(article);
            article.CreatedDate= DateTime.Now;
            return RedirectToAction("ArticleList");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            await GetCategories();
            var value = await _repository.GetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Article article)
        {
            await GetCategories();
            await _repository.UpdateAsync(article);
            return RedirectToAction("ArticleList");

        }
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction("ArticleList");
        }
    }
}
