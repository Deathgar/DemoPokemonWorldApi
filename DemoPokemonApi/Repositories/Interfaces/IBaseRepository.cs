using System.Linq.Expressions;

namespace DemoPokemonApi.Repositories.Interfaces
{
    public interface IBaseRepository<T>
    {
        IQueryable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> expression);
        Task<bool> Exist(int id);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
