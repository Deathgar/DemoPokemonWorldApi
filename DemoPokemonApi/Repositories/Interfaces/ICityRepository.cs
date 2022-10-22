using DemoPokemonApi.Models;
using System.Linq.Expressions;

namespace DemoPokemonApi.Repositories.Interfaces;

public interface ICityRepository : IBaseRepository<CityDto>
{
    Task<CityDto> GetByIdAsync(int id);
    Task CreateAsync(CityDto entity);
}
