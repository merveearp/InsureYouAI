using InsureYouAI.Entities;

namespace InsureYouAI.DTOs
{
    public class OpenAIArticleDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string MainCoverImageUrl { get; set; }
        public string CoverImageUrl { get; set; }

    }
}
