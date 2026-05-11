using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Areas.Admin.ViewComponents.AdminDashboard
{
    public class _DashboardRadialChartComponent : ViewComponent
    {
        private readonly InsureContext _context;

        public _DashboardRadialChartComponent(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var totalPolicyCount = _context.Policies.Count();

            ViewBag.TotalPolicyCount = totalPolicyCount;

            ViewBag.ActivePolicyCount = _context.Policies.Count(x => x.Status == "Active");
            ViewBag.PassivePolicyCount = _context.Policies.Count(x => x.Status == "Passive");
            ViewBag.KaskoPolicyCount = _context.Policies.Count(x => x.PolicyType == "Kasko");

            ViewBag.ActivePolicyRate = totalPolicyCount == 0 ? 0 :
                Math.Round((double)ViewBag.ActivePolicyCount * 100 / totalPolicyCount);

            ViewBag.PassivePolicyRate = totalPolicyCount == 0 ? 0 :
                Math.Round((double)ViewBag.PassivePolicyCount * 100 / totalPolicyCount);

            ViewBag.KaskoPolicyRate = totalPolicyCount == 0 ? 0 :
                Math.Round((double)ViewBag.KaskoPolicyCount * 100 / totalPolicyCount);

            ViewBag.TotalPolicyRate = 100;

            return View();
        }
    }
}