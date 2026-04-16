using InsureYouAI.Context;
using InsureYouAI.Entities;
using InsureYouAI.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAI.Repositories.PricingPlanItemRepositories
{
    public class PricingPlanItemRepository : GenericRepository<PricingPlanItem>, IPricingPlanItemRepository
    {
        public PricingPlanItemRepository(InsureContext context) : base(context)
        {
        }
    }
}
