using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Areas.Admin.ViewComponents.AdminDashboard
{
    public class _DashboardRoundChartComponent : ViewComponent
    {
        private readonly InsureContext _context;

        public _DashboardRoundChartComponent(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.ApprovedCommentCount = _context.Comments
                .Count(x => x.CommentStatus == "Yorum Onaylandı");

            ViewBag.PendingCommentCount = _context.Comments
                .Count(x => x.CommentStatus == "Onay Bekliyor");

            ViewBag.ToxicCommentCount = _context.Comments
                .Count(x => x.CommentStatus == "Toksik Yorum");

            ViewBag.TotalCommentCount =
                ViewBag.ApprovedCommentCount +
                ViewBag.PendingCommentCount +
                ViewBag.ToxicCommentCount;

            return View();
        }
    }
}