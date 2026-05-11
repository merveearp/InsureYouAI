using InsureYouAI.Areas.Admin.Models;
using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAI.Areas.Admin.ViewComponents.AdminDashboard
{
    public class _DashboardPolicyTypeComponent : ViewComponent
    {
        private readonly InsureContext _context;

        public _DashboardPolicyTypeComponent(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var totalPolicyCount = await _context.Policies.CountAsync();

            var values = await _context.Policies
                .GroupBy(x => x.PolicyType)
                .Select(x => new PolicyTypeModel
                {
                    PolicyType = x.Key,
                    Count = x.Count(),

                    Rate = totalPolicyCount == 0
                        ? 0
                        : Math.Round((double)x.Count() * 100 / totalPolicyCount, 1)
                })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            return View(values);
        }
    }
}