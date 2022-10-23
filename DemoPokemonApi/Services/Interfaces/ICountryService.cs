using DemoPokemonApi.ViewModels;

namespace DemoPokemonApi.Services.Interfaces;

public interface ICountryService : IBaseService<CountryViewModel>
{
    Task<IEnumerable<CityViewModel>> GetCitiesByCoutryAsync(int countryId);
    Task<IEnumerable<HabitatViewModel>> GetHabitatsByCoutryAsync(int countryId);
}
