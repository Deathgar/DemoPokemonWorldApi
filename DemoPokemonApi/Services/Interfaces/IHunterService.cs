using DemoPokemonApi.ViewModels;

namespace DemoPokemonApi.Services.Interfaces;

public interface IHunterService : IBaseService<HunterViewModel>
{
    Task<CityViewModel> GetCityAsync(int hunterId);
    Task<IEnumerable<PokemonViewModel>> GetPokemonsAsync(int hunterId);
}
