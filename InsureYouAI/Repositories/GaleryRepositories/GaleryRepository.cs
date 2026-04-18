using InsureYouAI.Context;
using InsureYouAI.Entities;
using InsureYouAI.Repositories.GenericRepository;

namespace InsureYouAI.Repositories.GaleryRepositories
{
    public class GaleryRepository : GenericRepository<Gallery>, IGaleryRepository
    {
        public GaleryRepository(InsureContext context) : base(context)
        {
        }
    }
}
