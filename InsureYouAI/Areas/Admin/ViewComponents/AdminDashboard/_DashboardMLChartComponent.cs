using InsureYouAI.Context;
using InsureYouAI.ML;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Areas.Admin.ViewComponents.AdminDashboard
{
    public class _DashboardMLChartComponent :ViewComponent
    {
        private readonly ForecastService _forecastService;
        private readonly InsureContext _context;

        public _DashboardMLChartComponent( InsureContext context)
        {
            _forecastService = new ForecastService();
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var monthlyData = _context.Policies
                .GroupBy(p => new
                {
                    Year = p.StartDate.Year,
                    Month = p.StartDate.Month
                })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    SalesCount = g.Count()
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToList();

            var salesData = monthlyData
                .Select(x => new PolicySalesData
                {
                    Date = new DateTime(x.Year, x.Month, 1),
                    SalesCount = x.SalesCount
                })
                .ToList();

            var actualSalesData = salesData
              .Where(x => x.Date.Year == 2025)
              .ToList();

            var forecast = _forecastService.GetForecast(actualSalesData, horizon: 12);

            var lastDate = actualSalesData.Last().Date;

            var forecastMonths = Enumerable.Range(1, 12)
                .Select(i => lastDate.AddMonths(i)
                .ToString("MMMM yyyy", new System.Globalization.CultureInfo("tr-TR")))
                .ToList();

            ViewBag.ActualMonths = actualSalesData
                .Select(x => x.Date.ToString("MMMM yyyy", new System.Globalization.CultureInfo("tr-TR")))
                .ToList();

            ViewBag.ActualValues = actualSalesData
                .Select(x => x.SalesCount)
                .ToList();

            ViewBag.ForecastMonths = forecastMonths;

            ViewBag.ForecastValues = forecast.ForecastedValues
                .Select(x => Math.Round(x, 0))
                .ToList();

            ViewBag.LowerValues = forecast.LowerBoundValues
                .Select(x => Math.Round(Math.Max(x, 0), 0))
                .ToList();

            ViewBag.UpperValues = forecast.UpperBoundValues
                .Select(x => Math.Round(x, 0))
                .ToList();

            return View();
        }
    }
}
