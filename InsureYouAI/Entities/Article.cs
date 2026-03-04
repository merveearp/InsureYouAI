namespace InsureYouAI.Entities
{
    public class Article
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; } 
        public string Content { get; set; }
        public string MainCoverImageUrl { get; set; }
        public string CoverImageUrl { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

    }
}
