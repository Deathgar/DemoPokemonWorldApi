using DemoPokemonApi.Models;
using DemoPokemonApi.ViewModels;

namespace DemoPokemonApi.Services.Interfaces;

public interface ICityService : IBaseService<CityViewModel>
{
    Task<IEnumerable<HunterViewModel>> GetHuntersAsync(int cityId);
    Task<CountryViewModel> GetCountryAsync(int cityId);
}
