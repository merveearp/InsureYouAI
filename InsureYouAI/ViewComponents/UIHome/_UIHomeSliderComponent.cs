using InsureYouAI.Repositories.SliderRepositories;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.UIHome
{
    public class _UIHomeSliderComponent :ViewComponent
    {
        private readonly ISliderRepository _sliderRepository;

        public _UIHomeSliderComponent(ISliderRepository sliderRepository)
        {
            _sliderRepository = sliderRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _sliderRepository.GetAllAsync();
            return View(values);
        }
    }
}
