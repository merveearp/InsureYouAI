using InsureYouAI.Entities;
using InsureYouAI.Repositories.FeatureRepositories;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FeatureController : Controller
    {
        private readonly IFeatureRepository _featureRepository;

        public FeatureController(IFeatureRepository featureRepository)
        {
            _featureRepository = featureRepository;
        }

        public async Task<IActionResult> FeatureList()
        {
            var values = await _featureRepository.GetAllAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Feature feature)
        {
            await _featureRepository.CreateAsync(feature);
            return RedirectToAction("FeatureList");
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var value = await _featureRepository.GetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Feature feature)
        {
            await _featureRepository.UpdateAsync(feature);
            return RedirectToAction("FeatureList");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _featureRepository.DeleteAsync(id);
            return RedirectToAction("FeatureList");
        }

    }
}
