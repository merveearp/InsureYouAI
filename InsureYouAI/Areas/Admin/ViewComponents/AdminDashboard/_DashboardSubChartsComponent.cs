using InsureYouAI.Context;
using InsureYouAI.Models;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Areas.Admin.ViewComponents.AdminDashboard
{
    public class _DashboardSubChartsComponent:ViewComponent
    {
        private readonly InsureContext _context;

        public _DashboardSubChartsComponent(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
