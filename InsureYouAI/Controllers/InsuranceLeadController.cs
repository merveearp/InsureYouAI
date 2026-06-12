using InsureYouAI.Context;
using InsureYouAI.DTOs.InsuranceLeadDtos;
using InsureYouAI.Entities;
using InsureYouAI.Services.ZohoServices;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Controllers
{
    public class InsuranceLeadController : Controller
    {
        private readonly InsureContext _context;
        private readonly IZohoService _zohoService;

        public InsuranceLeadController(InsureContext context, IZohoService zohoService)
        {
            _context = context;
            _zohoService = zohoService;
        }

        [HttpGet]
        public IActionResult CreateLead()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateLead(InsuranceLeadCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var lead = new InsuranceLead
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                InsuranceType = dto.InsuranceType,
                Message = dto.Message,
                CreatedDate = DateTime.UtcNow,
                IsSentToZoho = false,
                ZohoSyncStatus = "Pending"
            };

            _context.InsuranceLeads.Add(lead);

            await _context.SaveChangesAsync();

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

            TempData["SuccessMessage"] =
                "Teklif talebiniz başarıyla alındı. En kısa sürede sizinle iletişime geçeceğiz.";

            return RedirectToAction("CreateLead");
        }

      
    }
}