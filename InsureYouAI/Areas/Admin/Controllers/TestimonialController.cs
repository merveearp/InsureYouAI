using InsureYouAI.Entities;
using InsureYouAI.Repositories.TestimonialRepositories;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class TestimonialController : Controller
    {
        private readonly ITestimonialRepository _repository;

        public TestimonialController(ITestimonialRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> TestimonialList()
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
        public async Task<IActionResult> Create(Testimonial testimonial)
        {
            await _repository.CreateAsync(testimonial);
            return RedirectToAction("TestimonialList");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var value = await _repository.GetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Testimonial testimonial)
        {

            await _repository.UpdateAsync(testimonial);
            return RedirectToAction("TestimonialList");

        }
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction("TestimonialList");
        }
    }
}
