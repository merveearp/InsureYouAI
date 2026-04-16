using InsureYouAI.Entities;
using InsureYouAI.Repositories.PricingPlanItemRepositories;
using InsureYouAI.Repositories.PricingPlanRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InsureYouAI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PricingPlanItemController : Controller
    {
        private readonly IPricingPlanItemRepository _planItemRepository;
        private readonly IPricingPlanRepository _pricingPlanRepository;

        public PricingPlanItemController(IPricingPlanItemRepository pricingPlanItemRepository, IPricingPlanRepository pricingPlanRepository)
        {
            _planItemRepository = pricingPlanItemRepository;
            _pricingPlanRepository = pricingPlanRepository;
        }

        private async Task GetPlanList()
        {
            var values = await _planItemRepository.GetAllAsync();
            ViewBag.planList = (from plan in values
                                select new SelectListItem
                                {
                                    Text =plan.Title,
                                    Value=plan.PricingPlanItemId.ToString()
                                 });
        }

        public async Task<IActionResult> PricingPlanItemList()
        {
            var values = await _planItemRepository.GetAllAsync();
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await GetPlanList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PricingPlanItem plan)
        {
            await GetPlanList();
            await _planItemRepository.CreateAsync(plan);
            return RedirectToAction("PricingPlanItemList");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            await GetPlanList();
            var value = _planItemRepository.GetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PricingPlanItem plan)
        {
            await GetPlanList();
            await _planItemRepository.UpdateAsync(plan);
            return RedirectToAction("PricingPlanItemList");

        }
        public async Task<IActionResult> Delete(int id)
        {
            await _planItemRepository.DeleteAsync(id);
            return RedirectToAction("PricingPlanItemList");
        }
    }
}
