using DemoPokemonApi.Models;
using DemoPokemonApi.ViewModels;

namespace DemoPokemonApi.Services.Interfaces;

public interface ICityService : IBaseService<CityViewModel>
{
    Task<IEnumerable<HunterViewModel>> GetHuntersByCityAsync(int cityId);
    Task<CountryViewModel> GetCountryByCityAsync(int cityId);
}
