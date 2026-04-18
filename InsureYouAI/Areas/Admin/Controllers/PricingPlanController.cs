using InsureYouAI.Context;
using InsureYouAI.Entities;
using InsureYouAI.Repositories.PricingPlanItemRepositories;
using InsureYouAI.Repositories.PricingPlanRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAI.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class PricingPlanController : Controller
    {
        private readonly IPricingPlanRepository _repository;
        private readonly IPricingPlanItemRepository _itemRepository;
        private readonly InsureContext _context;

        public PricingPlanController(IPricingPlanRepository repository, InsureContext context, IPricingPlanItemRepository itemRepository)
        {
            _repository = repository;
            _context = context;
            _itemRepository = itemRepository;
        }

        public async Task<IActionResult> PricingPlanList()
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
        public async Task<IActionResult> Create(PricingPlan plan)
        {
            await _repository.CreateAsync(plan);
            return RedirectToAction("PricingPlanList");
        }

       
        [HttpPost]
        public async Task<IActionResult> Update(PricingPlan plan)
        {

            await _repository.UpdateAsync(plan);
            return RedirectToAction("PricingPlanList");

        }
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction("PricingPlanList");
        }

        [HttpGet]
        public async Task<IActionResult> GetPlan(int id)
        {
            var plan = await _context.PricingPlans
                .Include(x => x.PricingPlanItems)
                .FirstOrDefaultAsync(x => x.PricingPlanId == id);
            return View(plan);

        }


        [HttpPost]
        public async Task<IActionResult> CreateItem(PricingPlanItem planItem)
        {
            await _itemRepository.CreateAsync(planItem);

            return RedirectToAction("GetPlan", "PricingPlan", new { id = planItem.PricingPlanId });
        }


        [HttpPost]
        public async Task<IActionResult> UpdateItem(PricingPlanItem item)
        {
            var existingItem = await _context.PricingPlanItems
                .FirstOrDefaultAsync(x => x.PricingPlanItemId == item.PricingPlanItemId);

            if (existingItem == null)
                return NotFound();

            existingItem.Title = item.Title;

            await _context.SaveChangesAsync();

            return RedirectToAction("GetPlan", new { id = existingItem.PricingPlanId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);

            if (item == null)
                return NotFound();

            var planId = item.PricingPlan.PricingPlanId;

            await _itemRepository.DeleteAsync(id);

            return RedirectToAction("GetPlan", "PricingPlan", new { id = planId });
        }
    }
}
