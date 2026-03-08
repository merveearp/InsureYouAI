using InsureYouAI.Entities;
using InsureYouAI.Repositories.SliderRepositories;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class SliderController : Controller
    {
        private readonly ISliderRepository _repository;

        public SliderController(ISliderRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> SliderList()
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
        public async Task<IActionResult> Create(Slider slider)
        {
            await _repository.CreateAsync(slider);
            return RedirectToAction("SliderList");
        }

        [HttpGet]
        public  async Task<IActionResult> Update(int id)
        {
            var value = await _repository.GetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Slider slider)
        {

            await _repository.UpdateAsync(slider);
            return RedirectToAction("SliderList");
        }
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction("SliderList");
        }
    }
}
