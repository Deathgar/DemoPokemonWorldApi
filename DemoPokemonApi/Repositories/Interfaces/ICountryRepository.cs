using DemoPokemonApi.Models;

namespace DemoPokemonApi.Repositories.Interfaces;

public interface ICountryRepository : IBaseRepository<CountryDto>
{
    Task<CountryDto> GetByIdAsync(int id);
    Task<IEnumerable<CityDto>> GetCitiesByCountryAsync(int id);
    Task<IEnumerable<HabitatDto>> GetHabitatsByCountryAsync(int id);
}
