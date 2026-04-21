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
        public virtual List<Comment> Comments { get; set; }

        public int ViewCount { get; set; }
        public string? AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }

    }
}

