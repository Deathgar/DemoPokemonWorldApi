using DemoPokemonApi.ViewModels;

namespace DemoPokemonApi.Services.Interfaces;

public interface IPokemonService : IBaseService<PokemonViewModel>
{
    Task<IEnumerable<HunterViewModel>> GetHuntersAsync(int pokemonId);
    Task<HabitatViewModel> GetHabitatAsync(int pokemonId);
}
