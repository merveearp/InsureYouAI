using InsureYouAI.Context;
using InsureYouAI.Entities;
using InsureYouAI.Repositories.GenericRepository;

namespace InsureYouAI.Repositories.MessageRepositories
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(InsureContext context) : base(context)
        {
        }

        public async Task GetReadMessage(int id)
        {
            var value = await _context.Messages.FindAsync(id);
            value.IsRead = !value.IsRead;
            await _context.SaveChangesAsync();
        }
    }
}
