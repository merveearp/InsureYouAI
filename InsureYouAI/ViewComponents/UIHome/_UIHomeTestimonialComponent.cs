using InsureYouAI.Repositories.TestimonialRepositories;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.UIHome
{
    public class _UIHomeTestimonialComponent:ViewComponent
    {
        private readonly ITestimonialRepository _testimonialRepository;

        public _UIHomeTestimonialComponent(ITestimonialRepository testimonialRepository)
        {
            _testimonialRepository = testimonialRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _testimonialRepository.GetAllAsync();
            return View(values);
        }
    }
}
