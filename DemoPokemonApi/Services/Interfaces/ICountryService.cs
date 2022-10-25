using DemoPokemonApi.ViewModels;

namespace DemoPokemonApi.Services.Interfaces;

public interface ICountryService : IBaseService<CountryViewModel>
{
    Task<IEnumerable<CityViewModel>> GetCitiesAsync(int countryId);
    Task<IEnumerable<HabitatViewModel>> GetHabitatsAsync(int countryId);
}
