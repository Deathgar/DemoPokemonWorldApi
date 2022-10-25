using DemoPokemonApi.ViewModels;

namespace DemoPokemonApi.Services.Interfaces;

public interface IHabitatService : IBaseService<HabitatViewModel>
{
    Task<IEnumerable<CountryViewModel>> GetCountriesAsync(int habitatId);
    Task<IEnumerable<PokemonViewModel>> GetPokemonsAsync(int habitatId);
}
