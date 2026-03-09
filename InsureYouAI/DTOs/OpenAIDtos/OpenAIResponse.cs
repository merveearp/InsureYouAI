using static InsureYouAI.Areas.Admin.Controllers.ArticleController;

namespace InsureYouAI.DTOs.OpenAIDtos
{
    public class OpenAIResponse
    {
        public List<Choice> choices { get; set; }
    }
}
