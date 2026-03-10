using InsureYouAI.Entities;
using InsureYouAI.Repositories.AboutItemRepositories;
using InsureYouAI.Repositories.AboutRepositories;
using InsureYouAI.Services.GeminiServices;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class AboutItemController : Controller
    {
        private readonly IAboutItemRepository _repository;
        private readonly IGeminiService _geminiService;

        public AboutItemController(IAboutItemRepository repository, IGeminiService geminiService)
        {
            _repository = repository;
            _geminiService = geminiService;
        }

        public async Task<IActionResult> AboutItemList()
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
        public async Task<IActionResult> Create(AboutItem aboutItem)
        {
            await _repository.CreateAsync(aboutItem);
            return RedirectToAction("AboutItemList");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var value = await _repository.GetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AboutItem aboutItem)
        {

            await _repository.UpdateAsync(aboutItem);
            return RedirectToAction("AboutItemList");

        }
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction("AboutItemList");
        }

        [HttpGet]
        public async Task<IActionResult> GenerateAboutItem()
        {

            var result = await _geminiService.CreateAboutItem();
            return Json(result);

        }
    }
}
