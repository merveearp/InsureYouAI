using InsureYouAI.Context;
using InsureYouAI.Entities;
using InsureYouAI.Repositories.GenericRepository;

namespace InsureYouAI.Repositories.TrailerVideoRepositories
{
    public class TrailerVideoRepository : GenericRepository<TrailerVideo>, ITrailerVideoRepository
    {
        public TrailerVideoRepository(InsureContext context) : base(context)
        {
        }
    }
}
