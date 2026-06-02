namespace InsureYouAI.DTOs.InsuranceLeadDtos
{
    public class ZohoLeadResponseDto
    {
        public bool IsSuccess { get; set; }
        public string? ZohoLeadId { get; set; }
        public string? ErrorMessage { get; set; }
        public string? ResponseBody { get; set; }
    }
}
