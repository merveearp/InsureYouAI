using InsureYouAI.Entities;
using InsureYouAI.Repositories.GenericRepository;

namespace InsureYouAI.Repositories.MessageRepositories
{
    public interface IMessageRepository :IRepository<Message>
    {
        Task GetReadMessage(int id);
      
    }
}
