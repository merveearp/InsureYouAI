using InsureYouAI.Context;
using InsureYouAI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InsureYouAI.Areas.Admin.ViewComponents.AdminDashboard
{
    public class _DashboardSubCharts2Component :ViewComponent
    {
        private readonly InsureContext _context;
        public _DashboardSubCharts2Component(InsureContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var policyData = _context.Policies
                .GroupBy(p => p.PolicyType)
                .Select(g => new PolicyTypeCountViewModel
                {
                    PolicyType = g.Key,
                    Count = g.Count()

                }).ToList();

            ViewBag.policyData = JsonConvert.SerializeObject(policyData);
            return View();
        }
    }
}
