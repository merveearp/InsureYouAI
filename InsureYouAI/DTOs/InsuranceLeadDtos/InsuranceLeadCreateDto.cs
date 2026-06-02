namespace InsureYouAI.DTOs.InsuranceLeadDtos
{
    public class InsuranceLeadCreateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }

        public string InsuranceType { get; set; }
        public string Message { get; set; }
    }
}
