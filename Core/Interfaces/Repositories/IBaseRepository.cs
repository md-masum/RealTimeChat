using System.Linq.Expressions;
using Core.Entity;

namespace Core.Interfaces.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        //HTTP GET
        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity?> GetByIdAsync(Guid id);

        IQueryable<TEntity?> GetAsQueryable();

        Task<IList<TEntity>?> GetAllAsync();
        Task<IList<TEntity>?> GetAllAsync(Expression<Func<TEntity, bool>> predicate);

        //HTTP POST
        Task<bool> AddAsync(TEntity entity);
        Task<bool> AddListAsync(IList<TEntity> entity);

        //HTTP PUT
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> UpdateRangeAsync(IList<TEntity> entity);

        //HTTP DELETE
        Task<bool> RemoveAsync(TEntity entity);
        Task<bool> RemoveRangeAsync(IList<TEntity> entity);
    }
}
