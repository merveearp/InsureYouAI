using InsureYouAI.Repositories.TrailerVideoRepositories;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.UIHome
{
    public class _UIHomeVideoComponent :ViewComponent
    {
        private readonly ITrailerVideoRepository _trailerVideoRepository;

        public _UIHomeVideoComponent(ITrailerVideoRepository trailerVideoRepository)
        {
            _trailerVideoRepository = trailerVideoRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _trailerVideoRepository.GetAllAsync();
            return View(values);
        }
    }
}
