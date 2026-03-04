using InsureYouAI.Context;
using InsureYouAI.Entities;
using InsureYouAI.Repositories.GenericRepository;

namespace InsureYouAI.Repositories.PricingPlanRepositories
{
    public class PricingPlanRepository : GenericRepository<PricingPlan>, IPricingPlanRepository
    {
        public PricingPlanRepository(InsureContext context) : base(context)
        {
        }
    }
}
