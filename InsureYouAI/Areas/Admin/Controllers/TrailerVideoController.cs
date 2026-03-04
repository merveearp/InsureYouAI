using InsureYouAI.Entities;
using InsureYouAI.Repositories.TrailerVideoRepositories;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class TrailerVideoController : Controller
    {
        private readonly ITrailerVideoRepository _repository;

        public TrailerVideoController(ITrailerVideoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> TrailerVideoList()
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
        public async Task<IActionResult> Create(TrailerVideo video)
        {
            await _repository.CreateAsync(video);
            return RedirectToAction("TrailerVideoList");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var value = _repository.GetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> Update(TrailerVideo video)
        {

            await _repository.UpdateAsync(video);
            return RedirectToAction("TrailerVideoList");

        }
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction("TrailerVideoList");
        }
    }
}
