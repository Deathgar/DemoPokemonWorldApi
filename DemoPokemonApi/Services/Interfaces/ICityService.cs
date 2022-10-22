using DemoPokemonApi.Models;

namespace DemoPokemonApi.Services.Interfaces;

public interface ICityService
{
    Task<IEnumerable<City>> GetCitiesAsync();
    Task<City> GetCityAsync(int id);
    Task CreateCityAsync(City city);
    Task AddHunterToCityAsync(int cityId, Hunter hunter);
}
