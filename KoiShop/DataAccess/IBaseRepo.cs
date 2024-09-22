using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IBaseRepo<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(int id);               // Get entity by ID
        Task<IEnumerable<TEntity>> GetAllAsync();         // Get all entities
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);  // Find by condition

        Task AddAsync(TEntity entity);                    // Add new entity
        Task AddRangeAsync(IEnumerable<TEntity> entities);  // Add multiple entities

        void Update(TEntity entity);                      // Update an entity
        void Remove(TEntity entity);                      // Delete an entity
        void RemoveRange(IEnumerable<TEntity> entities);  // Delete multiple entities
    }
}
