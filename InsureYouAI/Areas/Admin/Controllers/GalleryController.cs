using InsureYouAI.Entities;
using InsureYouAI.Repositories.GaleryRepositories;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GalleryController : Controller
    {
        private readonly IGaleryRepository _galeryRepository;

        public GalleryController(IGaleryRepository galeryRepository)
        {
            _galeryRepository = galeryRepository;
        }

        public async Task<IActionResult> GalleryList()
        {
            var values = await _galeryRepository.GetAllAsync();
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Gallery gallery)
        {
            await _galeryRepository.CreateAsync(gallery);
            return RedirectToAction("GalleryList");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var value = await _galeryRepository.GetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Gallery gallery)
        {
           await _galeryRepository.UpdateAsync(gallery);
           return RedirectToAction("GalleryList");

        }

        public async Task<IActionResult> Delete(int id)
        {
            await _galeryRepository.DeleteAsync(id);
            return RedirectToAction("GalleryList");

        }

    }
}
