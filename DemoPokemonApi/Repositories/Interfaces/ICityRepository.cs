using DemoPokemonApi.Models;
using System.Linq.Expressions;

namespace DemoPokemonApi.Repositories.Interfaces;

public interface ICityRepository : IBaseRepository<City>
{
    Task<IEnumerable<City>> GetAllAsync(bool isInclude = true);
    Task<IEnumerable<City>> GetByConditionAsync(Expression<Func<City, bool>> expression, bool isInclude = true);
    Task<City> GetByIdAsync(int id, bool isInclude = true);
}
