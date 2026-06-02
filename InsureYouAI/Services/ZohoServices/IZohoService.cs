using InsureYouAI.DTOs.InsuranceLeadDtos;
using InsureYouAI.Entities;

namespace InsureYouAI.Services.ZohoServices
{
    public interface IZohoService
    {
        Task<ZohoLeadResponseDto> CreateLeadAsync(InsuranceLead lead);
    }
}
