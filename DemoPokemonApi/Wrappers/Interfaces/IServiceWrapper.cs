using DemoPokemonApi.Services.Interfaces;

namespace DemoPokemonApi.Wrappers.Interfaces;

public interface IServiceWrapper
{
    ICountryService CountryService { get; }
    ICityService CityService { get; }
    IHabitatService HabitatService { get; }
    IHunterService HunterService { get; }
    IPokemonService PokemonService { get; }
    IHunterLicenseService HunterLicenseService { get; }
}
