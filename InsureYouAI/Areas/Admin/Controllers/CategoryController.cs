
using InsureYouAI.Entities;
using InsureYouAI.Repositories.CategoryRepositories;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _repository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _repository = categoryRepository;
        }

        public async Task<IActionResult> CategoryList()
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
