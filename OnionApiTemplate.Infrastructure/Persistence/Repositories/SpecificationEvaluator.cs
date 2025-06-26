using Microsoft.EntityFrameworkCore;
using OnionApiTemplate.Domain.IRepositoty;

namespace OnionApiTemplate.Infrastructure.Persistence.Repositories
{
    internal class SpecificationEvaluator
    {
        public static IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, ISpecification<T> spec) where T : class
        {
            var query = inputQuery;

            // Apply includes
            if (spec.IncludeExpression != null && spec.IncludeExpression.Any())
                query = spec.IncludeExpression.Aggregate(query, (current, include) => current.Include(include));

            // Apply filter
            if (spec.Criteria != null)
                query = query.Where(spec.Criteria);

            // Apply ordering
            if (spec.OrderBy != null)
                query = query.OrderBy(spec.OrderBy);
            else if (spec.OrderByDescending != null)
                query = query.OrderByDescending(spec.OrderByDescending);

            // Apply pagination
            if (spec.IsPaginated)
                query = query.Skip(spec.Skip).Take(spec.Take);

            return query;
        }
    }
}
