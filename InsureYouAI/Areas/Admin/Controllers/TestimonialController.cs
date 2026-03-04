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

        public async Task<IActionResult> testimonialList()
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
            return RedirectToAction("testimonialList");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var value = _repository.GetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Testimonial testimonial)
        {

            await _repository.UpdateAsync(testimonial);
            return RedirectToAction("testimonialList");

        }
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction("testimonialList");
        }
    }
}
