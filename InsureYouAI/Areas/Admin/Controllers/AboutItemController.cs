using InsureYouAI.Entities;
using InsureYouAI.Repositories.AboutItemRepositories;
using InsureYouAI.Repositories.AboutRepositories;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class AboutItemController : Controller
    {
        private readonly IAboutItemRepository _repository;

        public AboutItemController(IAboutItemRepository repository)
        {
            _repository = repository;
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
        public IActionResult Update(int id)
        {
            var value = _repository.GetByIdAsync(id);
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
    }
}
