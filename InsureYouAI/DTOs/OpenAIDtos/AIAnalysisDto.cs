namespace InsureYouAI.DTOs.OpenAIDtos
{
    public class AIAnalysisDto
    {
        public string DamageType { get; set; }    
        public string Severity { get; set; }   
        public string Description { get; set; }   
        public string InsuranceStatus { get; set; }
        public string Recommendation { get; set; }
    }
}
