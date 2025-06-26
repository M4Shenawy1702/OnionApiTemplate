using Microsoft.EntityFrameworkCore;
using OnionApiTemplate.Domain.Entities;
using OnionApiTemplate.Domain.IRepositoty;

namespace OnionApiTemplate.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<TEntity, TKey>(ApplicationDbContext _context)
        : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly ApplicationDbContext _context = _context;

        public async Task AddAsync(TEntity entity) => await _context.Set<TEntity>().AddAsync(entity);
        public void Update(TEntity entity) => _context.Set<TEntity>().Update(entity);
        public void Delete(TEntity entity) => _context.Set<TEntity>().Remove(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool trackChanges)
            => trackChanges
                ? await _context.Set<TEntity>().ToListAsync()
                : await _context.Set<TEntity>().AsNoTracking().ToListAsync();

        public async Task<TEntity?> GetAsync(TKey key)
            => await _context.Set<TEntity>().FindAsync(key);

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> specification)
            => await SpecificationEvaluator.GetQuery(_context.Set<TEntity>(), specification).ToListAsync();

        public async Task<TEntity?> GetAsync(ISpecification<TEntity> specification)
            => await SpecificationEvaluator.GetQuery(_context.Set<TEntity>(), specification).FirstOrDefaultAsync();

        public async Task<int> GetCountAsync(ISpecification<TEntity> specification)
            => await SpecificationEvaluator.GetQuery(_context.Set<TEntity>(), specification).CountAsync();
    }
}
