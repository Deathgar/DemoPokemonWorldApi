using DemoPokemonApi.ViewModels;

namespace DemoPokemonApi.Services.Interfaces;

public interface IHabitatService : IBaseService<HabitatViewModel>
{
    Task<IEnumerable<CountryViewModel>> GetCountriesByHabitatAsync(int habitatId);
    Task<IEnumerable<PokemonViewModel>> GetPokemonsByHabitatAsync(int habitatId);
}
