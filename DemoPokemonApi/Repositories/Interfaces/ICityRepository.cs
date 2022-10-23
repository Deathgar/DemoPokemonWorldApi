using DemoPokemonApi.Models;
using System.Linq.Expressions;

namespace DemoPokemonApi.Repositories.Interfaces;

public interface ICityRepository : IBaseRepository<CityDto>
{
    Task<CityDto> GetByIdAsync(int id);
    Task<IEnumerable<HunterDto>> GetHuntersByCityAsync(int cityId);
    Task<CountryDto> GetCountryByCityAsync(int cityId);
}
