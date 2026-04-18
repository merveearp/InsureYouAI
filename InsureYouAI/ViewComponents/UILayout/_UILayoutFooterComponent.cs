using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.UILayout
{
    public class _UILayoutFooterComponent :ViewComponent
    {
        private readonly InsureContext _context;

        public _UILayoutFooterComponent(InsureContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.Info = _context.Contacts.Select(x => x.Desciption).FirstOrDefault();
            ViewBag.Address = _context.Contacts.Select(x => x.Address).FirstOrDefault();
            ViewBag.Email = _context.Contacts.Select(x => x.Email).FirstOrDefault();
            ViewBag.Phone = _context.Contacts.Select(x => x.PhoneNumber).FirstOrDefault();
            return View();
        }
    }
}
