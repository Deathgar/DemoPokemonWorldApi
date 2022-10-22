using DemoPokemonApi.ViewModels;

namespace DemoPokemonApi.Services.Interfaces;

public interface IPokemonService : IBaseService<PokemonViewModel>
{
    Task<IEnumerable<HunterViewModel>> GetHunters(int pokemonId);
    Task<HabitatViewModel> GetHabitat(int pokemonId);
}
