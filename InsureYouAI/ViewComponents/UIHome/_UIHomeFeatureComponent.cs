using InsureYouAI.Repositories.FeatureRepositories;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.UIHome
{
    public class _UIHomeFeatureComponent :ViewComponent
    {
        private readonly IFeatureRepository _featureRepository;

        public _UIHomeFeatureComponent(IFeatureRepository featureRepository)
        {
            _featureRepository = featureRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _featureRepository.GetAllAsync();
            return View(values);
        }
    }
}
