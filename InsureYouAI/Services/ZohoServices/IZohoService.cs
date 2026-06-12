using InsureYouAI.DTOs.InsuranceLeadDtos;
using InsureYouAI.Entities;

namespace InsureYouAI.Services.ZohoServices
{
    public interface IZohoService
    {
        Task<ZohoLeadResponseDto> CreateLeadAsync(InsuranceLead lead);

        Task<string> GetAccessTokenAsync();

        Task<string?> GetLeadStatusAsync(string zohoLeadId);

        Task<string?> GetLeadStatusAsync(string zohoLeadId, string accessToken);

        Task<bool> UpdateLeadStatusAsync(string zohoLeadId, string status);
        Task AddLeadNoteAsync(string zohoLeadId, string adminNote);
    }
}