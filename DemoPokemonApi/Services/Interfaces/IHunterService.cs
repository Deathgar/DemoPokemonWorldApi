using DemoPokemonApi.ViewModels;

namespace DemoPokemonApi.Services.Interfaces;

public interface IHunterService : IBaseService<HunterViewModel>
{
    Task<CityViewModel> GetCityByHunterAsync(int hunterId);
    Task<IEnumerable<PokemonViewModel>> GetPokemonsByHunterAsync(int hunterId);
}
