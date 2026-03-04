using InsureYouAI.Context;
using Microsoft.EntityFrameworkCore;

namespace InsureYouAI.Repositories.GenericRepository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly InsureContext _context;

        public GenericRepository(InsureContext context)
        {
            _context = context;
            
        }

        public async Task CreateAsync(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
           var value =  await _context.Set<T>().FindAsync(id);
           _context.Remove(value);
           await _context.SaveChangesAsync();

        }
        
        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);

        }

        public async Task UpdateAsync(T entity)
        {

            _context.Update(entity);
            await _context.SaveChangesAsync();

        }
    }
}
