using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Areas.Admin.ViewComponents.AdminDashboard
{
    public class _DashboardWidgetComponent:ViewComponent
    {
        private readonly InsureContext _context;

        public _DashboardWidgetComponent(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.v1 = _context.Categories.Count();
            ViewBag.v2 = _context.Revenues.Count();
            ViewBag.v3 = _context.Articles.Count();
            ViewBag.v4 = _context.Policies.Count();
            return View();
        }
    }
}
