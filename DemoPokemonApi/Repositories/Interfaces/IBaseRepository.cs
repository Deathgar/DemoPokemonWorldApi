using System.Linq.Expressions;

namespace DemoPokemonApi.Repositories.Interfaces
{
    public interface IBaseRepository<T>
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
