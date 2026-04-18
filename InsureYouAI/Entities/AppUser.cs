using Microsoft.AspNetCore.Identity;

namespace InsureYouAI.Entities
{
    public class AppUser :IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<AppUser> AppUsers { get; set; }

    }
}
