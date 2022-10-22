using DemoPokemonApi.Models;

namespace DemoPokemonApi.Repositories.Interfaces;

public interface ICountryRepository : IBaseRepository<CountryDto>
{
    Task<CountryDto> GetByIdAsync(int id);
    Task CreateAsync(CountryDto entity);
}
