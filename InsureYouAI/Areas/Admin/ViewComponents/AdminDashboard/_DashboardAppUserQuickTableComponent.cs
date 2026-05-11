using InsureYouAI.Areas.Admin.Models;
using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAI.Areas.Admin.ViewComponents.AdminDashboard
{
    public class _DashboardAppUserQuickTableComponent : ViewComponent
    {
        private readonly InsureContext _context;

        public _DashboardAppUserQuickTableComponent(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _context.Users
                .Select(x => new UserQuickTableModel
                {
                    FullName = x.Name + " " + x.Surname,
                    ImageUrl = x.ImageUrl,

                    PolicyCount = _context.Policies
                        .Count(p => p.AppUserId == x.Id),

                    TotalPremiumAmount = _context.Policies
                        .Where(p => p.AppUserId == x.Id)
                        .Sum(p => (decimal?)p.PremiumAmount) ?? 0
                })
                .OrderByDescending(x => x.PolicyCount)
                .Take(10)
                .ToListAsync();

            return View(values);
        }
    }
}