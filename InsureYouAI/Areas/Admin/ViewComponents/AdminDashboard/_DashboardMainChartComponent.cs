using InsureYouAI.Context;
using InsureYouAI.DTOs.ChartDtos;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace InsureYouAI.Areas.Admin.ViewComponents.AdminDashboard
{
    public class _DashboardMainChartComponent :ViewComponent
    {
        private readonly InsureContext _context;

        public _DashboardMainChartComponent(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            //Revenue
            var revenueData = _context.Revenues
                .GroupBy(r => r.ProcessDate.Month)
                .Select(x => new
                {
                    Month = x.Key,
                    TotalAmount = x.Sum(g => g.Amount)
                })
                .OrderBy(x => x.Month)
                .ToList();

            //Expense
            var expenseData = _context.Expenses
               .GroupBy(r => r.ExpenseDate.Month)
               .Select(x => new
               {
                   Month = x.Key,
                   TotalAmount = x.Sum(g => g.Amount)
               })
               .OrderBy(x => x.Month)
               .ToList();

            //AllMonths
            var allMonths = revenueData.Select(x => x.Month)
                .Union(expenseData.Select(y => y.Month))
                .OrderBy(x => x)
                .ToList();

            var model = new RevenueExpenseChartDto()
            {
                Months = allMonths.Select(x => new DateTimeFormatInfo()
                .GetAbbreviatedMonthName(x)).ToList(),
                RevenueTotals = allMonths.Select(x => revenueData.FirstOrDefault(m => m.Month == x)?.TotalAmount ?? 0).ToList(),
                ExpenseTotals = allMonths.Select(x => expenseData.FirstOrDefault(m => m.Month == x)?.TotalAmount ?? 0).ToList(),

            };

            ViewBag.v1 = _context.Revenues.Sum(x => x.Amount);
            ViewBag.v2 = _context.Expenses.Sum(x => x.Amount);

            return View(model);
        }
    }
}
