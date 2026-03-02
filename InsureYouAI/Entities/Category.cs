namespace InsureYouAI.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public IList<Article> Articles { get; set; }
    }
}
