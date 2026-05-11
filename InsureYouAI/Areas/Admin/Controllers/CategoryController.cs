
using InsureYouAI.Context;
using InsureYouAI.Entities;
using InsureYouAI.Repositories.CategoryRepositories;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _repository;
        private readonly InsureContext _context;

        public CategoryController(ICategoryRepository categoryRepository, InsureContext context)
        {
            _repository = categoryRepository;
            _context = context;
        }

        public async Task<IActionResult> CategoryList()
        {
            var values = await _repository.GetAllAsync();
            return View(values);
        }
        public IActionResult CategoryByArticleList(int id)
        {
            var value = _context.Articles.Where(x => x.CategoryId == id).Select(x=>x.Category.CategoryName).FirstOrDefault();
            ViewBag.CategoryName = value;

            var values = _context.Articles
            .Where(x => x.CategoryId == id)
            .ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            await _repository.CreateAsync(category);
            return RedirectToAction("CategoryList");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var value = await _repository.GetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Category category)
        {
           
            await _repository.UpdateAsync(category);
            return RedirectToAction("CategoryList");

        }
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id); 
            return RedirectToAction("CategoryList");
        }
    }
}
