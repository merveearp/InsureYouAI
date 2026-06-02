using InsureYouAI.Areas.Admin.Models;
using InsureYouAI.Context;
using InsureYouAI.Services.ZohoServices;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InsuranceLeadController : Controller
    {
        private readonly InsureContext _context;
        private readonly IZohoService _zohoService;

        public InsuranceLeadController(InsureContext context, IZohoService zohoService)
        {
            _context = context;
            _zohoService = zohoService;
        }

        public IActionResult Index()
        {
            var values = _context.InsuranceLeads
                .OrderByDescending(x => x.CreatedDate)
                .Select(x => new InsuranceLeadViewModel
                {
                    InsuranceLeadId = x.InsuranceLeadId,
                    FullName = x.FirstName + " " + x.LastName,
                    Email = x.Email,
                    Phone = x.Phone,
                    InsuranceType = x.InsuranceType,
                    ZohoSyncStatus = x.ZohoSyncStatus ?? "Beklemede",
                    IsSentToZoho = x.IsSentToZoho,
                    CreatedDate = x.CreatedDate,
                    ZohoLeadId = x.ZohoLeadId,
                    ZohoErrorMessage = x.ZohoErrorMessage
                })
                .ToList();

            return View(values);
        }

        [HttpPost]
        public async Task<IActionResult> SendToZoho(int id)
        {
            var lead = await _context.InsuranceLeads.FindAsync(id);

            if (lead == null)
            {
                return NotFound();
            }

            var zohoResult = await _zohoService.CreateLeadAsync(lead);

            if (zohoResult.IsSuccess)
            {
                lead.IsSentToZoho = true;
                lead.ZohoSyncStatus = "Success";
                lead.ZohoLeadId = zohoResult.ZohoLeadId;
                lead.ZohoErrorMessage = null;
            }
            else
            {
                lead.IsSentToZoho = false;
                lead.ZohoSyncStatus = "Failed";
                lead.ZohoErrorMessage = zohoResult.ErrorMessage;
            }

            _context.Update(lead);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public IActionResult LeadDetail(int id)
        {
            var value = _context.InsuranceLeads
                .Where(x => x.InsuranceLeadId == id)
                .Select(x => new InsuranceLeadViewModel
                {
                    InsuranceLeadId = x.InsuranceLeadId,
                    FullName = x.FirstName + " " + x.LastName,
                    Email = x.Email,
                    Phone = x.Phone,
                    InsuranceType = x.InsuranceType,
                    ZohoSyncStatus = x.ZohoSyncStatus,
                    IsSentToZoho = x.IsSentToZoho,
                    CreatedDate = x.CreatedDate,
                    ZohoLeadId = x.ZohoLeadId,
                    ZohoErrorMessage = x.ZohoErrorMessage,
                    Message = x.Message
                })
                .FirstOrDefault();

            return View(value);
        }
    }
}
