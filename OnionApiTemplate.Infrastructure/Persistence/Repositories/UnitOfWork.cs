using OnionApiTemplate.Domain.Entities;
using OnionApiTemplate.Domain.IRepositoty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionApiTemplate.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork(ApplicationDbContext _context)
        : IUnitOfWork, IAsyncDisposable
    {
        private readonly Dictionary<Type, object> _repositories = new();
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : BaseEntity<TKey>
        {
            var entityType = typeof(TEntity);

            if (!_repositories.TryGetValue(entityType, out var repository))
            {
                repository = new GenericRepository<TEntity, TKey>(_context);
                _repositories[entityType] = repository;
            }

            return (IGenericRepository<TEntity, TKey>)repository!;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public ValueTask DisposeAsync()
        {
            return _context.DisposeAsync();
        }
    }
}
