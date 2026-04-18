using InsureYouAI.Repositories.GaleryRepositories;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.UIHome
{
    public class _UIHomeGalleryComponent :ViewComponent
    {
        private readonly IGaleryRepository _galeryRepository;

        public _UIHomeGalleryComponent(IGaleryRepository galeryRepository)
        {
            _galeryRepository = galeryRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _galeryRepository.GetAllAsync();
            return View(values);
        }
    }
}
