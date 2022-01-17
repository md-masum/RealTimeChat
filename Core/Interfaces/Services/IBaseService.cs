using System.Linq.Expressions;
using Core.Entity;

namespace Core.Interfaces.Services
{
    public interface IBaseService<TEntity, TDto> where TEntity : BaseEntity
    {
        //HTTP GET LIST
        Task<IList<TDto>> GetAllAsync();
        Task<IList<TDto>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IList<TEntity>> GetAllEntityAsync();
        Task<IList<TDto>> GetTop(int number);

        //HTTP GET SINGLE
        Task<TDto> GetByIdAsync(Guid id);
        Task<TDto> GetAsync(Expression<Func<TEntity, bool>> predicate);

        //HTTP POST
        Task<TDto> AddAsync(TDto entity);

        //HTTP PUT
        Task<TDto> UpdateAsync(TDto entity);

        //HTTP DELETE
        Task<bool> RemoveAsync(Guid id);
    }
}
