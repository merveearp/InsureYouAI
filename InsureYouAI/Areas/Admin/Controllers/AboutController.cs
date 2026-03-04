using InsureYouAI.Context;
using InsureYouAI.Entities;

using InsureYouAI.Repositories.AboutRepositories;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class AboutController : Controller
    {

        private readonly IAboutRepository _repository;

        public AboutController(IAboutRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> AboutList()
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
        public async Task<IActionResult> Create(About about)
        {
            await _repository.CreateAsync(about);
            return RedirectToAction("AboutList");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var value = await _repository.GetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> Update(About about)
        {

            await _repository.UpdateAsync(about);
            return RedirectToAction("AboutList");

        }
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction("AboutList");
        }
    }
}

