using InsureYouAI.Context;
using InsureYouAI.Entities;
using InsureYouAI.Repositories.GenericRepository;

namespace InsureYouAI.Repositories.AboutItemRepositories
{
    public class AboutItemRepository : GenericRepository<AboutItem>, IAboutItemRepository
    {
        public AboutItemRepository(InsureContext context) : base(context)
        {
        }
    }
}
