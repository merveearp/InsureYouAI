using InsureYouAI.DTOs.OpenAIDtos;
using InsureYouAI.Entities;
using InsureYouAI.Repositories.ArticleRepositories;
using InsureYouAI.Repositories.CategoryRepositories;
using InsureYouAI.Services.OpenAIServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InsureYouAI.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ArticleController : Controller
    {
        private readonly IArticleRepository _repository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IOpenAIService _openAIService;


        public ArticleController(IArticleRepository repository, ICategoryRepository categoryRepository, IOpenAIService openAIService)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _openAIService = openAIService;
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
        public async Task<IActionResult> Create()
        {
            await GetCategories();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Article article)
        {
            await GetCategories();
            article.CreatedDate = DateTime.Now;
            await _repository.CreateAsync(article);
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

           
            var existing = await _repository.GetByIdAsync(article.ArticleId);

            if (existing == null) return NotFound();

          
            existing.Title = article.Title;
            existing.Content = article.Content;
            existing.CategoryId = article.CategoryId;
            existing.MainCoverImageUrl = article.MainCoverImageUrl;
            existing.CoverImageUrl = article.CoverImageUrl;

            await _repository.UpdateAsync(existing);
            return RedirectToAction("ArticleList");
        }
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction("ArticleList");
        }

        [HttpGet]
        public IActionResult CreateArticleWithOpenAI()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticleWithOpenAI(string prompt)
        {
            var article = await _openAIService.CreateArticleWithAI(prompt);

            ViewBag.article = article;

            return View();
        }
       
    }
}
